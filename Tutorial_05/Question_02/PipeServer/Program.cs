using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace Tutorial_05.Question_02.PipeServer
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Named Pipe Server ===");
            Console.WriteLine($"Process ID: {Environment.ProcessId}");
            Console.WriteLine("Waiting for client connection...\n");

            using (NamedPipeServerStream server =
                new NamedPipeServerStream(
                    "demo_pipe",
                    PipeDirection.InOut,
                    1,
                    PipeTransmissionMode.Byte,
                    PipeOptions.None))
            {
                server.WaitForConnection();
                Console.WriteLine("Client connected.\n");

                Console.WriteLine("Waiting to receive message...");
                
                byte[] buffer = new byte[1024];
                int bytesRead = server.Read(buffer, 0, buffer.Length);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).TrimEnd('\0');

                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine($"Received: {message}");
                    string response = $"Server processed: {message}";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    server.Write(responseBytes, 0, responseBytes.Length);
                    server.Flush();
                    Console.WriteLine("Response sent.");
                }
            }

            Console.WriteLine("\nServer finished.");
            Console.ReadLine();
        }
    }
}
