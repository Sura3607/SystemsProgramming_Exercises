using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Tutorial_05.Question_04.RpcServer
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
            Console.WriteLine("=== RPC Server ===");
            TcpListener listener = new TcpListener(IPAddress.Loopback, 6000);
            listener.Start();
            Console.WriteLine("Listening on port 6000...\n");

            using TcpClient client = listener.AcceptTcpClient();
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            string json = reader.ReadLine();
            Console.WriteLine($"Received JSON: {json}");

            RpcRequest request = JsonSerializer.Deserialize<RpcRequest>(json);

            RpcResponse response = new RpcResponse();

            if (request.Method == "MoneyExchange")
            {
                response.Result = MoneyExchange(request.Currency, request.Amount);
                response.Message = "Success";
            }
            else
            {
                response.Message = "Unknown method";
            }

            string responseJson = JsonSerializer.Serialize(response);
            writer.WriteLine(responseJson);

            Console.WriteLine($"Sent response: {responseJson}");

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
