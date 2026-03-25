using Confluent.Kafka;
using System.Text.Json;
using PnlEngine.Models;

namespace PnlEngine.Producer
{
    public class PnlUpdateKafkaProducer : IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public PnlUpdateKafkaProducer(IConfiguration config)
        {
            string bootstrapServers = config["Kafka:BootstrapServers"]
                ?? throw new InvalidOperationException("Kafka:BootstrapServers is missing");

            string topic = config["Kafka:PnlUpdateTopic"]
                ?? throw new InvalidOperationException("Kafka:PnlUpdateTopic is missing");

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                Acks = Acks.All,
                EnableIdempotence = true
            };

            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
            _topic = topic;
        }

        public async Task PublishAsync(PnLUpdate update)
        {
            var message = new Message<string, string>
            {
                Key = update.User,
                Value = JsonSerializer.Serialize(update)
            };

            await _producer.ProduceAsync(_topic, message);
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(5));
            _producer.Dispose();
        }
    }
}