using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Tutorial_04.Question_04
{
    internal class Q4_main
    {
        static readonly string WatchDir =
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Question_04", "Input");

        static readonly string OutputDir =
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Question_04", "Output");

        static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3); 

        public static void Run()
        {
            Directory.CreateDirectory(WatchDir);
            Directory.CreateDirectory(OutputDir);

            Console.WriteLine("=== File Monitoring and Concurrent Processing ===");
            Console.WriteLine($"Watch folder : {WatchDir}");
            Console.WriteLine($"Output folder: {OutputDir}\n");

            FileSystemWatcher watcher = new FileSystemWatcher(WatchDir);
            watcher.Created += OnFileCreated;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Drop sample files (in Data folder) into Input folder to start processing...");
            Console.ReadLine();
        }

        static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Task.Run(async () =>
            {
                await _semaphore.WaitAsync();
                try
                {
                    await ProcessFileAsync(e.FullPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
                finally
                {
                    _semaphore.Release();
                }
            });
        }

        static async Task ProcessFileAsync(string filePath)
        {
            // tránh đọc file khi OS chưa ghi xong
            await Task.Delay(200);

            string fileName = Path.GetFileName(filePath);
            string outputPath = Path.Combine(OutputDir, fileName + ".gz");

            Console.WriteLine($"Processing {fileName} | T{Thread.CurrentThread.ManagedThreadId}");

            using FileStream input = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using FileStream output = new FileStream(outputPath, FileMode.Create);
            using GZipStream gzip = new GZipStream(output, CompressionMode.Compress);

            await input.CopyToAsync(gzip);

            Console.WriteLine($"Done {fileName}");
        }
    }
}
