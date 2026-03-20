using Confluent.Kafka;

namespace PnlEngine.Consumers
{
    internal class EquityPriceConsumer
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly string EQUITY_TOPIC = "";

        public EquityPriceConsumer() 
        {
            var config = new ConsumerConfig
            {
                GroupId = "equity-price-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public async Task ConsumeEquityPricesAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(EQUITY_TOPIC);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume();

                    Console.WriteLine($"Recieved: {result.Message.Value}");

                    //
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _consumer.Close();
            }
        }
    } 
}
