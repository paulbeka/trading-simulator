using PnlEngine.Interfaces;
using PnlEngine.Models;
using PnlEngine.Producer;

public class PnlService
{
    private readonly IPriceStore _priceStore;
    private readonly ITickerUserIndex _tickerIndex;
    private readonly IUserPositionStore _positions;
    private readonly IPnlStore _pnlStore;
    private readonly PnlUpdateKafkaProducer _producer;

    public PnlService(
        IPriceStore priceStore,
        ITickerUserIndex tickerIndex,
        IUserPositionStore positions,
        IPnlStore pnlStore,
        PnlUpdateKafkaProducer producer)
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

            var delta = (price - position.LastPrice) * position.Quantity;

            await _pnlStore.ChangePnlWithDelta(user, delta);

            var totalPnl = await _pnlStore.Get(user);
            var positionPnl = (price - position.EntryPrice) * position.Quantity;

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

            position.LastPrice = price;
            await _positions.Set(user, ticker, position);
        }
    }
}