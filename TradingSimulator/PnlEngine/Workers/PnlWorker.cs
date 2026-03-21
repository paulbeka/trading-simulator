using PnlEngine.Consumers;
using PnlEngine.Services;

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
            await _initialisationService.LoadPositions();
            await _initialisationService.LoadPrices();

            _pnlService.InitialiseUserPnL(_initialisationService.GetAllUsers());

            var priceTask = _priceConsumer.ConsumeEquityPricesAsync(cancellationToken);

            await Task.WhenAll(priceTask);
        }
    }
}
