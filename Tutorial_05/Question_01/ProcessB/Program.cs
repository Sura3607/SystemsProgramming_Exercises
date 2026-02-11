using System;
using System.Threading;

namespace Tutorial_05.Question_01.ProcessB
{
    internal class Program
    {
        static int sharedData = 999;

        static void Main()
        {
            Console.WriteLine("=== Process B (Consumer) ===");
            Console.WriteLine($"Process ID : {Environment.ProcessId}");
            Console.WriteLine($"Initial sharedData = {sharedData}");

            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"Process B reads: {sharedData}");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Process B finished.");
            Console.ReadLine();
        }
    }
}