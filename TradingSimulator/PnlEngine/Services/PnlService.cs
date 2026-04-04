using PnlEngine.Interfaces;
using PnlEngine.Models;
using PnlEngine.Producer;
using PnlEngine.Producer.Interfaces;

public class PnlService
{
    private readonly IPriceStore _priceStore;
    private readonly ITickerUserIndex _tickerIndex;
    private readonly IUserPositionStore _positions;
    private readonly IPnlStore _pnlStore;
    private readonly IPnlUpdateProducer _producer;

    public PnlService(
        IPriceStore priceStore,
        ITickerUserIndex tickerIndex,
        IUserPositionStore positions,
        IPnlStore pnlStore,
        IPnlUpdateProducer producer)
    {
        _priceStore = priceStore;
        _tickerIndex = tickerIndex;
        _positions = positions;
        _pnlStore = pnlStore;
        _producer = producer;
    }

    public async Task HandlePriceUpdate(string ticker, decimal price)
    {
        var (changed, oldPrice) = await _priceStore.UpdateIfChanged(ticker, price);

        if (!changed || oldPrice == null)
            return;

        var users = await _tickerIndex.GetUsersFromTicker(ticker);

        foreach (var user in users)
        {
            var position = await _positions.Get(user, ticker);
            if (position == null) continue;

            var delta = (price - oldPrice.Value) * position.Quantity;

            await _pnlStore.ChangePnlWithDelta(user, delta);

            var totalPnl = await _pnlStore.Get(user);

            var positionPnl = (price - position.AvgEntryPrice) * position.Quantity;

            await _producer.PublishAsync(new PnLUpdate
            {
                User = user,
                Ticker = ticker,
                Price = price,
                PositionPnL = positionPnl,
                PositionDelta = delta,
                TotalPnL = totalPnl,
                TotalDelta = delta,
            });
        }
    }
}