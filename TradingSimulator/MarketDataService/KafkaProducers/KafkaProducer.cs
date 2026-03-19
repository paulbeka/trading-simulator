using Confluent.Kafka;

namespace MarketDataService.KafkaProducers
{
    internal class KafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProducer() 
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishAsync(string topic, string data)
        {
            await _producer.ProduceAsync(topic, new Message<Null, string>
            {
                Value = data
            });
        }
    }
}
