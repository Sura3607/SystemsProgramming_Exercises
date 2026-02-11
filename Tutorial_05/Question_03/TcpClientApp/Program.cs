using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Tutorial_05.Question_03.TcpClientApp
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("=== TCP Client ===");
            Console.WriteLine($"Process ID: {Environment.ProcessId}\n");

            string host = "127.0.0.1";
            int port = 5000;

            using (TcpClient client = new TcpClient())
            {
                Console.WriteLine("Connecting to server...");
                client.Connect(host, port);
                Console.WriteLine("Connected.\n");

                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                {
                    string message = "Hello via TCP";

                    Console.WriteLine("Sending request...");
                    writer.WriteLine(message);

                    Console.WriteLine("Waiting for response...");
                    string response = reader.ReadLine();

                    Console.WriteLine($"Received: {response}");
                }
            }

            Console.WriteLine("\nClient finished.");
            Console.ReadLine();
        }
    }
}
