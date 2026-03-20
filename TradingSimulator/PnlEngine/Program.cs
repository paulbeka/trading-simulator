using PnlEngine.Stores;
using PnlEngine.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<PriceCache>();
builder.Services.AddSingleton<UserToTickerIndex>();
builder.Services.AddSingleton<TickerToUserIndex>();

builder.Services.AddHostedService<PnlWorker>();

var host = builder.Build();
host.Run();
