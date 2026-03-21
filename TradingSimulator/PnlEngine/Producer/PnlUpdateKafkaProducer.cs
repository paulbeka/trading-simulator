using Confluent.Kafka;
using System.Text.Json;
using PnlEngine.Models;

namespace PnlEngine.Producer
{
    internal class PnlUpdateKafkaProducer : IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public PnlUpdateKafkaProducer(string bootstrapServers, string topic)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                Acks = Acks.All,
                EnableIdempotence = true
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
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