using Communication;
using DemoWorker;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole();

builder.Services.AddCommunication(typeof(MessageHandler));
builder.Services.AddCommunicationMessageHandler();
builder.Services.AddCommunicationRequestHandler();

var app = builder.Build();

app.UseCommunicationRequestMiddleware();

Console.WriteLine("Starting delay to hack a guarantee of rabbit");
await Task.Delay(10000);
Console.WriteLine("Finished delay");

app.Run();
