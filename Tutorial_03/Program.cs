using System;
using Tutorial_03.Question_01;
using Tutorial_03.Question_02;
using Tutorial_03.Question_03;
using Tutorial_03.Question_04;
using Tutorial_03.Question_05;


namespace Tutorial_03
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== TUTORIAL 2 ===");
                Console.WriteLine("1. Question 01");
                Console.WriteLine("2. Question 02");
                Console.WriteLine("3. Question 03");
                Console.WriteLine("3. Question 04");
                Console.WriteLine("5. Question 05");
                Console.WriteLine("A. Run All");
                Console.WriteLine("ESC. ");
                Console.WriteLine("---------------------------");
                Console.Write("Press key: ");

                var input = Console.ReadKey();

                switch (input.Key)
                {
                    case ConsoleKey.Escape:
                        return;

                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        RunSingleQuestion(1);
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        RunSingleQuestion(2);
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        RunSingleQuestion(3);
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        RunSingleQuestion(4);
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        RunSingleQuestion(5);
                        break;

                    case ConsoleKey.A:
                        RunAllQuestions();
                        break;

                    default:
                        break;
                }
            }
        }


        static void RunSingleQuestion(int number)
        {
            Console.Clear();
            Console.WriteLine($"--- Running Question {number} ---\n");

            switch (number)
            {
                case 1: Q1_main.Run(); break;
                case 2: Q2_main.Run(); break;
                case 3: Q3_main.Run(); break;
                case 4: Q4_main.Run(); break;
                case 5: Q5_main.Run(); break;
            }

            Console.WriteLine("\n------------------------------------------------");
            Console.WriteLine("Execution completed. Press any key to return to the menu...");
            Console.ReadKey();
        }

        static void RunAllQuestions()
        {
            Console.Clear();
            Console.WriteLine("=== RUN ALL ===\n");

            Console.WriteLine(">>> Question 01:");
            Q1_main.Run();
            WaitABit();

            Console.Clear();
            Console.WriteLine("\n>>> Question 02:");
            Q2_main.Run();
            WaitABit();

            Console.Clear();
            Console.WriteLine("\n>>> Question 03:");
            Q3_main.Run();
            WaitABit();

            Console.Clear();
            Console.WriteLine("\n>>> Question 04:");
            Q4_main.Run();
            WaitABit();

            Console.Clear();
            Console.WriteLine("\n>>> Question 05:");
            Q5_main.Run();

            Console.WriteLine("\n------------------------------------------------");
            Console.WriteLine("Execution completed. Press any key to return to the menu...");
            Console.ReadKey();
        }

        static void WaitABit()
        {
            Console.WriteLine("\n(waiting... ...)");
            Thread.Sleep(3000);
            Console.WriteLine("---------------------------------\n");
        }
    
    }
}