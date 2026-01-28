using System;


namespace Tutorial_03.Question_01
{
    internal class Q1_main
    {
        struct PointStruct
        {
            public int X;
            public int Y;
        }

        class PersonClass
        {
            public string Name;
        }
        static void AllocateInScope()
        {
            // Biến local nằm trên Stack
            int localInt = 5;

            // Đối tượng Heap mới.
            // Khi hàm này kết thúc, không còn biến nào tham chiếu đến 'tempPerson'.
            PersonClass tempPerson = new PersonClass { Name = "Bob" };

            Console.WriteLine($"Inside Scope: Created {tempPerson.Name}");
        }

        public static void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Stack vs Heap ===");

            // STACK: Biến 'a' (int) được lưu trực tiếp trên Stack
            int a = 10;

            // STACK: Toàn bộ dữ liệu của struct (X, Y) nằm trên Stack
            PointStruct p = new PointStruct { X = 1, Y = 2 };

            // HEAP & STACK: 
            // - Đối tượng PersonClass thực sự được cấp phát trên HEAP.
            // - Biến tham chiếu 'person' (chứa địa chỉ bộ nhớ) nằm trên STACK.
            PersonClass person = new PersonClass { Name = "Alice" };

            AllocateInScope();

            // Tại đây, các đối tượng trong AllocateInScope đã ra khỏi phạm vi.
            // Đối tượng trên Heap tạo trong hàm đó đã đủ điều kiện để Garbage Collection (GC).
            Console.WriteLine("end of Q1_Run");
        }
    }
}
