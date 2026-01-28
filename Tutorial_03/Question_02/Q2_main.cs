using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_03.Question_02
{
    internal class Q2_main
    {
        class LargeObject
        {
            public byte[] Data = new byte[1024 * 10]; // 10KB
        }

        public static void Run()
        {
            Console.WriteLine("=== Garbage Collection ===");

            long beforeMemory = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory Before: {beforeMemory} bytes");

            for (int i = 0; i < 5000; i++)
            {
                LargeObject obj = new LargeObject();
                // obj là biến tạm, sau mỗi vòng lặp, đối tượng cũ trở thành rác (Gen 0)
            }

            long allocatedMemory = GC.GetTotalMemory(false);
            Console.WriteLine($"Memory Allocated (Before GC): {allocatedMemory} bytes");

            // Chạy GC
            Console.WriteLine("Forcing GC...");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            long afterMemory = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory After GC: {afterMemory} bytes");
        }
    }
}
