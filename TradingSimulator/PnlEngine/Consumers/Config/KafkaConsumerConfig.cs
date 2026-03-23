namespace PnlEngine.Consumers.Config
{
    public class KafkaConsumerConfig
    {
        public string BootstrapServers { get; set; } = "";
        public string EquityTopic { get; set; } = "";
        public string GroupId { get; set; } = "";
    }
}
