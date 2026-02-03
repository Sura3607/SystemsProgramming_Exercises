using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Tutorial_04.Question_03
{
    internal class Q3_main
    {
        static readonly string FilePath = "log.txt";
        static readonly object _lock = new object();

        public static void Run()
        {
            Console.WriteLine("=== Thread-Safe File Access ===\n");

            Run_NoSync();
            Run_WithLock();
            Run_WithQueue();
        }

        static void Run_NoSync()
        {
            Console.WriteLine("=== No synchronization ===");
            File.WriteAllText(FilePath, string.Empty);

            var sw = Stopwatch.StartNew();

            Parallel.For(0, 50, i =>
            {
                File.AppendAllText(FilePath, $"Log from Task {i}\n");
            });

            sw.Stop();
            Console.WriteLine($"Time(ms): {sw.ElapsedMilliseconds}\n");
        }

        static void Run_WithLock()
        {
            Console.WriteLine("=== lock ===");
            File.WriteAllText(FilePath, string.Empty);

            var sw = Stopwatch.StartNew();

            Parallel.For(0, 50, i =>
            {
                lock (_lock)
                {
                    File.AppendAllText(FilePath, $"Log from Task {i}\n");
                }
            });

            sw.Stop();
            Console.WriteLine($"Time(ms): {sw.ElapsedMilliseconds}\n");
        }

        static void Run_WithQueue()
        {
            Console.WriteLine("=== Dedicated logging task ===");
            File.WriteAllText(FilePath, string.Empty);

            var sw = Stopwatch.StartNew();
            BlockingCollection<string> queue = new BlockingCollection<string>();

            Task logger = Task.Run(() =>
            {
                using var writer = new StreamWriter(FilePath, append: true);
                foreach (var msg in queue.GetConsumingEnumerable())
                {
                    writer.WriteLine(msg);
                }
            });

            Parallel.For(0, 50, i =>
            {
                queue.Add($"Log from Task {i}");
            });

            queue.CompleteAdding();
            logger.Wait();

            sw.Stop();
            Console.WriteLine($"Time(ms): {sw.ElapsedMilliseconds}\n");
        }
    }
}
