using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Tutorial_05.Question_05.JsonRpcClient
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("=== JSON-RPC Client ===\n");

            using TcpClient client = new TcpClient("127.0.0.1", 7000);
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            // Request 1: Successful - MoneyExchange method exists
            var request1 = new
            {
                jsonrpc = "2.0",
                method = "MoneyExchange",
                @params = new { currency = "USD", amount = 100 },
                id = "1"
            };

            string json1 = JsonSerializer.Serialize(request1);
            Console.WriteLine($"[Request 1 - Successful]");
            Console.WriteLine($"Sending: {json1}");
            writer.WriteLine(json1);

            string response1 = reader.ReadLine();
            Console.WriteLine($"Received: {response1}");
            
            var responseObj1 = JsonSerializer.Deserialize<JsonElement>(response1);
            if (responseObj1.TryGetProperty("result", out JsonElement result))
            {
                Console.WriteLine($"✓ Success: Result = {result}\n");
            }
            else if (responseObj1.TryGetProperty("error", out JsonElement error))
            {
                Console.WriteLine($"✗ Error: {error}\n");
            }

            // Request 2: Failed - Method does not exist
            var request2 = new
            {
                jsonrpc = "2.0",
                method = "InvalidMethod",
                @params = new { data = "test" },
                id = "2"
            };

            string json2 = JsonSerializer.Serialize(request2);
            Console.WriteLine($"[Request 2 - Method Not Found]");
            Console.WriteLine($"Sending: {json2}");
            writer.WriteLine(json2);

            string response2 = reader.ReadLine();
            Console.WriteLine($"Received: {response2}");
            
            var responseObj2 = JsonSerializer.Deserialize<JsonElement>(response2);
            if (responseObj2.TryGetProperty("result", out JsonElement result2))
            {
                Console.WriteLine($"✓ Success: Result = {result2}\n");
            }
            else if (responseObj2.TryGetProperty("error", out JsonElement error2))
            {
                int code = error2.GetProperty("code").GetInt32();
                string message = error2.GetProperty("message").GetString();
                Console.WriteLine($"✗ Error Code: {code}, Message: {message}\n");
            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
