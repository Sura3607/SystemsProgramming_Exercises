using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Tutorial_04.Question_02
{
    internal class Q2_main
    {
        public static void Run()
        {
            Console.WriteLine("=== Task Coordination and Synchronization ===\n");

            Run_WhenAll();
            Run_CountdownEvent();
            Run_ManualResetEventSlim();
        }

        static void Run_WhenAll()
        {
            Console.WriteLine("=== Task.WhenAll ===");

            var sw = Stopwatch.StartNew();

            Task[] tasks =
            {
                SimulateWork(1),
                SimulateWork(2),
                SimulateWork(3)
            };

            Task.WhenAll(tasks).Wait();

            sw.Stop();
            Console.WriteLine($"All tasks finished | Time(ms): {sw.ElapsedMilliseconds}\n");
        }

        static void Run_CountdownEvent()
        {
            Console.WriteLine("=== CountdownEvent ===");

            var sw = Stopwatch.StartNew();
            using CountdownEvent countdown = new CountdownEvent(3);

            for (int i = 1; i <= 3; i++)
            {
                int id = i;
                Task.Run(() =>
                {
                    SimulateWorkSync(id);
                    countdown.Signal();
                });
            }

            countdown.Wait();
            sw.Stop();

            Console.WriteLine($"All tasks finished | Time(ms): {sw.ElapsedMilliseconds}\n");
        }

        static void Run_ManualResetEventSlim()
        {
            Console.WriteLine("=== ManualResetEventSlim ===");

            var sw = Stopwatch.StartNew();
            ManualResetEventSlim doneEvent = new ManualResetEventSlim(false);
            int completed = 0;

            for (int i = 1; i <= 3; i++)
            {
                int id = i;
                Task.Run(() =>
                {
                    SimulateWorkSync(id);
                    if (Interlocked.Increment(ref completed) == 3)
                        doneEvent.Set();
                });
            }

            doneEvent.Wait();
            sw.Stop();

            Console.WriteLine($"All tasks finished | Time(ms): {sw.ElapsedMilliseconds}\n");
        }

        static Task SimulateWork(int id)
        {
            return Task.Run(() =>
            {
                int delay = Random.Shared.Next(300, 800);
                Console.WriteLine($"Task {id} running ({delay} ms)");
                Thread.Sleep(delay);
                Console.WriteLine($"Task {id} done");
            });
        }

        static void SimulateWorkSync(int id)
        {
            int delay = Random.Shared.Next(300, 800);
            Console.WriteLine($"Task {id} running ({delay} ms)");
            Thread.Sleep(delay);
            Console.WriteLine($"Task {id} done");
        }
    }
}
