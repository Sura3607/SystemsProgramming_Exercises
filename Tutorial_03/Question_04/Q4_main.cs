using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Tutorial_03.Question_04
{
    internal class Q4_main
    {
        static ConcurrentDictionary<int, bool> threadIds = new();

        static void DoWork()
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            threadIds.TryAdd(id, true);

            Thread.Sleep(10);
            Console.WriteLine($"Thread ID: {id}");
        }

        static long Measure(string title, Action action, out int uniqueThreads)
        {
            threadIds.Clear();

            Stopwatch sw = Stopwatch.StartNew();
            action();
            sw.Stop();

            uniqueThreads = threadIds.Count;
            Console.WriteLine($"Unique Thread IDs: {uniqueThreads}");

            return sw.ElapsedMilliseconds;
        }

        public static void Run()
        {
            long t1, t2, t3;
            int c1, c2, c3;

            Console.WriteLine("=== Multithreading Measurement ===");

            t1 = Measure("Thread", () =>
            {
                List<Thread> list = new();

                for (int i = 0; i < 100; i++)
                {
                    Thread t = new Thread(DoWork);
                    list.Add(t);
                    t.Start();
                }

                foreach (var t in list)
                    t.Join();
            }, out c1);

            t2 = Measure("ThreadPool", () =>
            {
                using CountdownEvent cde = new(100);

                for (int i = 0; i < 100; i++)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        DoWork();
                        cde.Signal();
                    });
                }

                cde.Wait();
            }, out c2);

            t3 = Measure("Task", () =>
            {
                Task[] tasks = new Task[100];

                for (int i = 0; i < 100; i++)
                    tasks[i] = Task.Run(DoWork);

                Task.WaitAll(tasks);
            }, out c3);

            Console.WriteLine("\n=== Summary ===");
            Console.WriteLine("Method\t\tTime(ms)\tUnique Threads");

            Console.WriteLine($"Thread\t\t{t1}\t\t{c1}");
            Console.WriteLine($"ThreadPool\t{t2}\t\t{c2}");
            Console.WriteLine($"Task\t\t{t3}\t\t{c3}");
        }
    }
}
