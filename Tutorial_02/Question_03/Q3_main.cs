using System;
using System.Diagnostics;

namespace Tutorial_02.Question_03
{
    internal class Q3_main
    {
        static int HotMethod(int x)
        {
            int sum = 0;
            for (int i = 0; i < 10_000; i++)
                sum += x * i;
            return sum;
        }

        public static void Run()
        {
            Console.WriteLine("=== Just-In-Time (JIT) Compilation ===\n");
            Stopwatch sw = new Stopwatch();

            // First call (JIT here)
            sw.Start();
            HotMethod(5);
            sw.Stop();
            long firstCall = sw.ElapsedTicks;
            Console.WriteLine($"First call (with JIT): {firstCall} ticks");

            // Second call
            sw.Restart();
            HotMethod(5);
            sw.Stop();
            long secondCall = sw.ElapsedTicks;
            Console.WriteLine($"Second call (native): {secondCall} ticks");

            // Third call 
            sw.Restart();
            HotMethod(5);
            sw.Stop();
            long thirdCall = sw.ElapsedTicks;
            Console.WriteLine($"Third call (native): {thirdCall} ticks");
        }
    }
}
