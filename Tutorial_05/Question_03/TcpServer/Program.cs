using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Tutorial_05.Question_03.TcpServer
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("=== TCP Server ===");
            Console.WriteLine($"Process ID: {Environment.ProcessId}");

            int port = 5000;
            TcpListener listener = new TcpListener(IPAddress.Loopback, port);

            listener.Start();
            Console.WriteLine($"Listening on port {port}...\n");

            using (TcpClient client = listener.AcceptTcpClient())
            {
                Console.WriteLine("Client connected.\n");

                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                {
                    Console.WriteLine("Waiting for request...");
                    string request = reader.ReadLine();

                    Console.WriteLine($"Received: {request}");

                    string response = $"Server processed: {request}";
                    writer.WriteLine(response);

                    Console.WriteLine("Response sent.");
                }
            }

            listener.Stop();
            Console.WriteLine("\nServer finished.");
            Console.ReadLine();
        }
    }
}
