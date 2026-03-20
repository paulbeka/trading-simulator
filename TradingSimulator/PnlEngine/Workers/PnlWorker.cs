namespace PnlEngine.Workers
{
    internal class PnlWorker : BackgroundService
    {
        private readonly PriceConsumer _priceConsumer;
        private readonly PositionConsumer _positionConsumer;

        public PnLWorker(
            PriceConsumer priceConsumer,
            PositionConsumer positionConsumer)
        {
            _priceConsumer = priceConsumer;
            _positionConsumer = positionConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var priceTask = _priceConsumer.ConsumeEquityPrices(stoppingToken);

            await Task.WhenAll(priceTask);
        }
    }
}
