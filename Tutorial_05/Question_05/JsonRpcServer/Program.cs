using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Tutorial_05.Question_05.JsonRpcServer
{
    public class JsonRpcRequest
    {
        public string jsonrpc { get; set; }
        public string method { get; set; }
        public JsonElement @params { get; set; }
        public string id { get; set; }
    }

    public class JsonRpcResponse
    {
        public string jsonrpc { get; set; } = "2.0";
        public object result { get; set; }
        public RpcError error { get; set; }
        public string id { get; set; }
    }

    public class RpcError
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("=== JSON-RPC Server ===");
            TcpListener listener = new TcpListener(IPAddress.Loopback, 7000);
            listener.Start();
            Console.WriteLine("Listening on port 7000...\n");

            using TcpClient client = listener.AcceptTcpClient();
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            Console.WriteLine("Client connected.\n");

            while (true)
            {
                string json = reader.ReadLine();
                if (string.IsNullOrEmpty(json))
                    break;

                Console.WriteLine($"Received: {json}");

                JsonRpcRequest request = JsonSerializer.Deserialize<JsonRpcRequest>(json);
                JsonRpcResponse response = new JsonRpcResponse { id = request.id };

                try
                {
                    switch (request.method)
                    {
                        case "MoneyExchange":
                            string currency = request.@params.GetProperty("currency").GetString();
                            double amount = request.@params.GetProperty("amount").GetDouble();
                            response.result = MoneyExchange(currency, amount);
                            break;

                        case "Add":
                            double a = request.@params.GetProperty("a").GetDouble();
                            double b = request.@params.GetProperty("b").GetDouble();
                            response.result = a + b;
                            break;

                        default:
                            response.error = new RpcError
                            {
                                code = -32601,
                                message = "Method not found"
                            };
                            break;
                    }
                }
                catch (Exception ex)
                {
                    response.error = new RpcError
                    {
                        code = -32603,
                        message = ex.Message
                    };
                }

                string responseJson = JsonSerializer.Serialize(response);
                writer.WriteLine(responseJson);
                Console.WriteLine($"Sent: {responseJson}\n");
            }

            Console.WriteLine("Client disconnected.");
            listener.Stop();
            Console.ReadLine();
        }

        static double MoneyExchange(string currency, double amount)
        {
            double rate = currency switch
            {
                "USD" => 24000,
                "EUR" => 26000,
                _ => 1
            };

            return amount * rate;
        }
    }
}
