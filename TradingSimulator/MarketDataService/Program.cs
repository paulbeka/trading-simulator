using MarketDataService.KafkaProducers;
using MarketDataService.Providers;
using MarketDataService.Redis;
using MarketDataService.Redis.Interfaces;
using MarketDataService.Workers;
using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

// Redis
var redisConnection = builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379";
var redis = ConnectionMultiplexer.Connect(redisConnection);

builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddSingleton<ITickerStore, RedisTickerStore>();

builder.Services.AddSingleton<PolygonProvider>();
builder.Services.AddSingleton<KafkaProducer>();
builder.Services.AddHostedService<MarketDataWorker>();

var app = builder.Build();
app.Run();
