using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection; 

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();