using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Tutorial_05.Question_02.PipeClient
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Named Pipe Client ===");
            Console.WriteLine($"Process ID: {Environment.ProcessId}\n");

            using (NamedPipeClientStream client =
                new NamedPipeClientStream(".", "demo_pipe", PipeDirection.InOut))
            {
                Console.WriteLine("Connecting to server...");
                client.Connect(5000);
                Console.WriteLine("Connected.\n");

                string message = "Hello from Client";
                Console.WriteLine("Sending message...");
                
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                client.Write(messageBytes, 0, messageBytes.Length);
                client.Flush();

                Console.WriteLine("Waiting for response...");
                
                byte[] buffer = new byte[1024];
                int bytesRead = client.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead).TrimEnd('\0');

                Console.WriteLine($"Received: {response}");
            }

            Console.WriteLine("\nClient finished.");
            Console.ReadLine();
        }
    }
}
