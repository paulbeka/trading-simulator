namespace PnlEngine.Workers
{
    internal class PnlWorker : BackgroundService
    {
        private readonly EquityPriceConsumer _priceConsumer;

        public PnlWorker(EquityPriceConsumer priceConsumer)
        {
            _priceConsumer = priceConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var priceTask = _priceConsumer.ConsumeEquityPricesAsync(cancellationToken);

            await Task.WhenAll(priceTask);
        }
    }
}
