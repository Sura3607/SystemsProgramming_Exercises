using System;
using System.Diagnostics;

namespace Tutorial_02.Question_02
{
    internal class Q2_main
    {
        struct MyStruct
        {
            public int A;
            public int B;
        }

        class MyClass
        {
            public int A;
            public int B;
        }

        public static void Run()
        {
            const int size = 10_000_000;
            Console.WriteLine("=== Struct vs Class: Memory & Performance ===");

            // ===== STRUCT ARRAY =====
            Console.WriteLine("\nStarting measurement for struct array...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memBeforeStruct = GC.GetTotalMemory(true);
            MyStruct[] structArray = new MyStruct[size];
            long memAfterStruct = GC.GetTotalMemory(true);

            Console.WriteLine($"Struct array memory: {(memAfterStruct - memBeforeStruct) / (1024 * 1024)} MB");

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < size; i++)
            {
                structArray[i].A++;
            }
            sw.Stop();
            Console.WriteLine($"Struct access time: {sw.ElapsedMilliseconds} ms");

            structArray = null;

            // ===== CLASS ARRAY =====
            Console.WriteLine("\nStarting measurement for class array...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long memBeforeClass = GC.GetTotalMemory(true);
            MyClass[] classArray = new MyClass[size];
            for (int i = 0; i < size; i++)
            {
                classArray[i] = new MyClass();
            }
            long memAfterClass = GC.GetTotalMemory(true);

            Console.WriteLine($"Class array memory: {(memAfterClass - memBeforeClass) / (1024 * 1024)} MB");

            sw.Restart();
            for (int i = 0; i < size; i++)
            {
                classArray[i].A++;
            }
            sw.Stop();
            Console.WriteLine($"Class access time: {sw.ElapsedMilliseconds} ms");

            classArray = null;
        }
        public static void RunAverage(int iterations = 10)
        {
            const int size = 10_000_000;

            Console.WriteLine($"=== BENCHMARK ({iterations} RUNS) ===");
            Console.WriteLine($"Array size: {size:N0}\n");

            // ================= STRUCT =================
            long structAlloc = 0;
            long structTime = 0;

            Console.WriteLine(">>> STRUCT");

            for (int i = 0; i < iterations; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                long beforeAlloc = GC.GetAllocatedBytesForCurrentThread();

                var sw = Stopwatch.StartNew();

                MyStruct[] arr = new MyStruct[size];
                for (int j = 0; j < size; j++)
                    arr[j].A++;

                sw.Stop();

                long afterAlloc = GC.GetAllocatedBytesForCurrentThread();

                structAlloc += (afterAlloc - beforeAlloc);
                structTime += sw.ElapsedMilliseconds;

                arr = null;
                Console.Write(".");
            }

            Console.WriteLine(" Done.\n");

            // ================= CLASS =================
            long classAlloc = 0;
            long classTime = 0;

            Console.WriteLine(">>> CLASS");

            for (int i = 0; i < iterations; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                long beforeAlloc = GC.GetAllocatedBytesForCurrentThread();

                var sw = Stopwatch.StartNew();

                MyClass[] arr = new MyClass[size];
                for (int j = 0; j < size; j++)
                    arr[j] = new MyClass();

                for (int j = 0; j < size; j++)
                    arr[j].A++;

                sw.Stop();

                long afterAlloc = GC.GetAllocatedBytesForCurrentThread();

                classAlloc += (afterAlloc - beforeAlloc);
                classTime += sw.ElapsedMilliseconds;

                arr = null;
                Console.Write(".");
            }

            Console.WriteLine(" Done.\n");

            // ================= RESULT =================
            Console.WriteLine("========= RESULT (AVERAGE) =========");
            Console.WriteLine("Type    | Allocated (MB) | Time (ms)");
            Console.WriteLine("------------------------------------");

            Console.WriteLine(
                $"STRUCT  | {(structAlloc / iterations) / 1024d / 1024d:F2}          | {(double)structTime / iterations:F2}");

            Console.WriteLine(
                $"CLASS   | {(classAlloc / iterations) / 1024d / 1024d:F2}         | {(double)classTime / iterations:F2}");

            Console.WriteLine("====================================");
        }
    }
}
