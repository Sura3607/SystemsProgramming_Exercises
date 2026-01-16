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
    }
}
