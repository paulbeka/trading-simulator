using PnlEngine.Consumers.Config;
using PnlEngine.Interfaces;
using PnlEngine.Producer;
using PnlEngine.Redis;
using PnlEngine.Workers;
using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

var redisConnection = builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379";
var redis = ConnectionMultiplexer.Connect(redisConnection); 
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

builder.Services.AddSingleton<IPriceStore, RedisPriceStore>();
builder.Services.AddSingleton<IUserPositionStore, RedisUserPositionStore>();
builder.Services.AddSingleton<ITickerUserIndex, RedisTickerUserIndex>();
builder.Services.AddSingleton<IPnlStore, RedisPnlStore>();
builder.Services.AddSingleton<ITickerStore, RedisTickerStore>();

builder.Services.AddSingleton<PnlService>();
builder.Services.AddSingleton<PnlUpdateKafkaProducer>();

builder.Services.AddSingleton<EquityPriceConsumer>();
builder.Services.Configure<KafkaConsumerConfig>(
    builder.Configuration.GetSection("Kafka")
);

builder.Services.AddHostedService<PnlWorker>();

var host = builder.Build();
host.Run();