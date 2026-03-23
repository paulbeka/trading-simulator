using Confluent.Kafka;
using Microsoft.Extensions.Options;
using PnlEngine.Consumers.Config;
using PnlEngine.Services;

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
}