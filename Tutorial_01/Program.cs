using System;
using System.IO;
using System.Runtime.InteropServices; 

namespace Tutorial_01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // tiếng Việt
            Console.WriteLine("=== TUTORIAL 1 ===");

            OperatingSystem osVersion = Environment.OSVersion;
            Console.WriteLine("\nHệ điều hành:");
            Console.WriteLine($"- Platform        : {osVersion.Platform}");
            Console.WriteLine($"- Version         : {osVersion.Version}");
            Console.WriteLine($"- Version String  : {osVersion.VersionString}");

            string netVersion = RuntimeInformation.FrameworkDescription;
            Console.WriteLine($"Phiên bản .NET Runtime: {netVersion}");

            bool is64BitOS = Environment.Is64BitOperatingSystem;
            bool is64BitProcess = Environment.Is64BitProcess;
            Console.WriteLine("\nKiến trúc:");
            Console.WriteLine($"- Hệ điều hành    : {(is64BitOS ? "64-bit" : "32-bit")}");
            Console.WriteLine($"- Tiến trình     : {(is64BitProcess ? "64-bit" : "32-bit")}");

            int processorCount = Environment.ProcessorCount;
            Console.WriteLine($"\nSố lõi CPU: {processorCount}");

            long totalMemory = GC.GetTotalMemory(false);
            Console.WriteLine($"\nBộ nhớ đang dùng: {totalMemory / 1024:N0} KB");

            string currentDirectory = Environment.CurrentDirectory;
            Console.WriteLine($"\nThư mục hiện tại: {currentDirectory}");

            DateTime localTime = DateTime.Now;
            DateTime utcTime = DateTime.UtcNow;
            Console.WriteLine("\nThời gian:");
            Console.WriteLine($"- Local Time : {localTime:dd/MM/yyyy HH:mm:ss}");
            Console.WriteLine($"- UTC Time   : {utcTime:dd/MM/yyyy HH:mm:ss}");

            Console.WriteLine("\n------------------------------------------------");


            Console.WriteLine("\nNhấn phím bất kỳ để thoát...");
            Console.ReadKey();
        }
    }
}