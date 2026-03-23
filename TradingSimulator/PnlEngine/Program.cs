using PnlEngine.Consumers;
using PnlEngine.Consumers.Config;
using PnlEngine.Producer;
using PnlEngine.Services;
using PnlEngine.Stores;
using PnlEngine.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<PnlStore>();
builder.Services.AddSingleton<PriceCache>();
builder.Services.AddSingleton<UserToTickerIndex>();
builder.Services.AddSingleton<TickerToUserIndex>();
builder.Services.AddSingleton<EquityPriceConsumer>();
builder.Services.AddSingleton<PnlService>();
builder.Services.AddSingleton<UserPositions>();
builder.Services.AddSingleton<PnlUpdateKafkaProducer>();

builder.Services.Configure<KafkaConsumerConfig>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddHostedService<PnlWorker>();

var host = builder.Build();
host.Run();
