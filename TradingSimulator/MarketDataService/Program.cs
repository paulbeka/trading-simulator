using MarketDataService.KafkaProducers;
using MarketDataService.Providers;
using MarketDataService.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<FinnhubProvider>();
builder.Services.AddSingleton<KafkaProducer>();
builder.Services.AddHostedService<MarketDataWorker>();

var app = builder.Build();
app.Run();
