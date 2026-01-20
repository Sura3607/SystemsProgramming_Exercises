using System;

namespace Tutorial_02.Question_01
{
    internal class Q1_main
    {
        struct PointStruct
        {
            public int X;
            public int Y;
        }

        class PointClass
        {
            public int X;
            public int Y;
        }

        public static void Run()
        {
            Console.WriteLine("=== Value Types và Reference Types ===\n");
            PointStruct origStruck = new PointStruct { X = 10, Y = 20 };
            PointClass origClass = new PointClass { X = 10, Y = 20 };
            Console.WriteLine("\n-- Original Values --");
            Console.WriteLine($"PointStruct: X={origStruck.X}, Y={origStruck.Y}");
            Console.WriteLine($"PointClass: X={origClass.X}, Y={origClass.Y}");

            Console.WriteLine("\n-- Copy Original --");
            PointStruct copyStruct = origStruck;
            PointClass copyClass = origClass;
            Console.WriteLine($"PointStruct Copy: X={copyStruct.X}, Y={copyStruct.Y}");
            Console.WriteLine($"PointClass Copy: X={copyClass.X}, Y={copyClass.Y}");

            Console.WriteLine("\n--> Change Value X Copy to 999 <--");
            copyStruct.X = 999;
            Console.WriteLine("\n-PointStruc-");
            Console.WriteLine($"PointStruct Original: X={origStruck.X}, Y={origStruck.Y}");
            Console.WriteLine($"PointStruct Copy: X={copyStruct.X}, Y={copyStruct.Y}");

            copyClass.X = 999;
            Console.WriteLine("\n-PointClass-");
            Console.WriteLine($"PointClass Original: X={origClass.X}, Y={origClass.Y}");
            Console.WriteLine($"PointClass Copy: X={copyClass.X}, Y={copyClass.Y}");

            Console.WriteLine("\nPress any key to continue...");
        }
    }
}
