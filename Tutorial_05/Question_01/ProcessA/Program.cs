using System;
using System.Threading;

namespace Tutorial_05.Question_01.ProcessA
{
    internal class Program
    {
        static int sharedData = 0;

        static void Main()
        {
            Console.WriteLine("=== Process A (Producer) ===");
            Console.WriteLine($"Process ID : {Environment.ProcessId}");
            Console.WriteLine($"Initial sharedData = {sharedData}");

            for (int i = 1; i <= 5; i++)
            {
                sharedData = i * 10;
                Console.WriteLine($"Process A produced: {sharedData}");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Process A finished.");
            Console.ReadLine();
        }
    }
}
