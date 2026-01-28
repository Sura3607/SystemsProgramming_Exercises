using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tutorial_03.Question_03
{
    internal class Q3_main
    {
        static void RunAndMeasure(string title, Action action)
        {
            Console.WriteLine($"\n=== {title} ===");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long beforeMemory = GC.GetAllocatedBytesForCurrentThread();

            Stopwatch sw = Stopwatch.StartNew();
            action();
            sw.Stop();

            long afterMemory = GC.GetAllocatedBytesForCurrentThread();

            Console.WriteLine($"Time (ms): {sw.ElapsedMilliseconds}");
            Console.WriteLine($"Allocated (bytes): {afterMemory - beforeMemory}");
        }

        static void Process(string value)
        {
            // giả lập xử lý
            if (value.Length > 0) { }
        }

        static void ProcessSpan(ReadOnlySpan<char> span)
        {
            if (span.Length > 0) { }
        }

        public static void Run()
        {
            Console.WriteLine("=== Memory & Performance Measurement ===");

            string data = "User:1001,User:1002,User:1003,User:1004";

            //LINQ + List
            RunAndMeasure("Method 1: LINQ + List<T>", () =>
            {
                var list = data
                    .Split(',')
                    .Where(x => x.StartsWith("User"))
                    .ToList();

                foreach (var u in list)
                    Process(u);
            });

            //Array + for
            RunAndMeasure("Method 2: Array + for loop", () =>
            {
                string[] arr = data.Split(',');

                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].StartsWith("User"))
                        Process(arr[i]);
                }
            });

             //Span<T>

            RunAndMeasure("Method 3: ReadOnlySpan<char>", () =>
            {
                ReadOnlySpan<char> spanData = data.AsSpan();

                int start = 0;
                int commaIndex;

                while ((commaIndex = spanData.Slice(start).IndexOf(',')) != -1)
                {
                    ReadOnlySpan<char> segment = spanData.Slice(start, commaIndex);
                    ProcessSpan(segment);
                    start += commaIndex + 1;
                }

                if (start < spanData.Length)
                {
                    ProcessSpan(spanData.Slice(start));
                }
            });
        }
    }
}
