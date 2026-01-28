using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tutorial_03.Question_05
{
    internal class Q5_main
    {
        static async Task LongRunningOperationAsync()
        {
            Console.WriteLine($"Start - Thread {Thread.CurrentThread.ManagedThreadId}");

            await Task.Delay(2000); // giả lập I/O hoặc tác vụ chờ

            Console.WriteLine($"End   - Thread {Thread.CurrentThread.ManagedThreadId}");
        }

        static async void AsyncVoidMethod()
        {
            await Task.Delay(1000);
            throw new Exception("Async void exception"); //async void làm lỗi runtime khó kiểm soát
        }

        public static async Task Run()
        {
            Console.WriteLine("=== Async/Await Demo ===");

            await LongRunningOperationAsync();

            try
            {
                AsyncVoidMethod();
            }
            catch
            {
                Console.WriteLine("This will never be caught");
            }

            Task t = LongRunningOperationAsync();

            // Deadlock
            // t.Wait();
            // var result = t.Result;

            await Task.Delay(3000);
        }
    }
}
