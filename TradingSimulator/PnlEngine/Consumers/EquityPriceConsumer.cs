using Confluent.Kafka;
using Microsoft.Extensions.Options;
using PnlEngine.Consumers.Config;
using PnlEngine.Models;
using PnlEngine.Services;
using System.Text.Json;

internal class EquityPriceConsumer
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly PnlService _pnlService;
    private readonly string _topic;

    public EquityPriceConsumer(PnlService pnlService, IOptions<KafkaConsumerConfig> options)
    {
        _pnlService = pnlService;

        var kafka = options.Value;

        var config = new ConsumerConfig
        {
            BootstrapServers = kafka.BootstrapServers,
            GroupId = kafka.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _topic = kafka.EquityTopic;

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    public async Task ConsumeEquityPricesAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                HandlePriceUpdate(_consumer.Consume());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            _consumer.Close();
        }
    }

    private void HandlePriceUpdate(ConsumeResult<Ignore, string> message)
    {
        Console.WriteLine($"Recieved: {message.Message.Value}");

        var priceUpdate = JsonSerializer.Deserialize<PriceUpdate>(message.Message.Value);

        if (priceUpdate != null)
        {
            _pnlService.HandlePriceUpdate(priceUpdate.Ticker, priceUpdate.Price);
        }
    }
}