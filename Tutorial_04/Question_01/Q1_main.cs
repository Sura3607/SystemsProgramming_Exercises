using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Tutorial_04.Question_01
{
    internal class Q1_main
    {
        static int counter;
        static readonly object _lock = new object();

        public static void Run()
        {
            Console.WriteLine("===== Race Conditions and Thread Safety =====");

            Console.WriteLine("\n--- Race Condition ---");
            RunTest("No sync", Increment_NoSync);

            Console.WriteLine("\n--- Fixed with lock ---");
            RunTest("lock", Increment_Lock);

            Console.WriteLine("\n--- Fixed with Interlocked ---");
            RunTest("Interlocked", Increment_Interlocked);
        }

        static void RunTest(string name, Action incrementMethod)
        {
            counter = 0;
            int taskCount = 5;
            int iterations = 100_000;

            var sw = Stopwatch.StartNew();
            Task[] tasks = new Task[taskCount];

            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int j = 0; j < iterations; j++)
                        incrementMethod();
                });
            }

            Task.WaitAll(tasks);
            sw.Stop();

            Console.WriteLine($"{name,-12} | Counter = {counter} | Time(ms) = {sw.ElapsedMilliseconds}");
        }

        static void Increment_NoSync()
        {
            counter++; // race condition
        }

        static void Increment_Lock()
        {
            lock (_lock)
            {
                counter++;
            }
        }

        static void Increment_Interlocked()
        {
            Interlocked.Increment(ref counter);
        }
    }
}
