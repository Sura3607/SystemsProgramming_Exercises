using Microsoft.Extensions.Hosting;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();