using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Tutorial_05.Question_04.RpcClient
{
    public class RpcRequest
    {
        public string Method { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
    }

    public class RpcResponse
    {
        public double Result { get; set; }
        public string Message { get; set; }
    }

    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("=== RPC Client ===\n");

            using TcpClient client = new TcpClient("127.0.0.1", 6000);
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            RpcRequest request = new RpcRequest
            {
                Method = "MoneyExchange",
                Currency = "USD",
                Amount = 100
            };

            string json = JsonSerializer.Serialize(request);

            Console.WriteLine($"Sending JSON: {json}");
            writer.WriteLine(json);

            string responseJson = reader.ReadLine();
            Console.WriteLine($"Received JSON: {responseJson}");

            RpcResponse response = JsonSerializer.Deserialize<RpcResponse>(responseJson);

            Console.WriteLine($"\nExchange Result: {response.Result}");
            Console.WriteLine($"Status: {response.Message}");

            Console.ReadLine();
        }
    }
}
