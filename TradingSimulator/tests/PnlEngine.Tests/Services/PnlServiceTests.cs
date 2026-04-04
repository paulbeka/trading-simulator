using Moq;
using PnlEngine.Interfaces;
using PnlEngine.Models;
using PnlEngine.Producer;
using PnlEngine.Producer.Interfaces;

public class PnlServiceTests
{
    private readonly Mock<IPriceStore> _priceStore = new();
    private readonly Mock<ITickerUserIndex> _tickerIndex = new();
    private readonly Mock<IUserPositionStore> _positions = new();
    private readonly Mock<IPnlStore> _pnlStore = new();
    private readonly Mock<IPnlUpdateProducer> _producer = new();

    private readonly PnlService _service;

    public PnlServiceTests()
    {
        _service = new PnlService(
            _priceStore.Object,
            _tickerIndex.Object,
            _positions.Object,
            _pnlStore.Object,
            _producer.Object
        );
    }

    [Fact]
    public async Task HandlePriceUpdate_NoChange_DoesNothing()
    {
        _priceStore
            .Setup(x => x.UpdateIfChanged("AAPL", 100))
            .ReturnsAsync((false, 90m));

        await _service.HandlePriceUpdate("AAPL", 100);

        _tickerIndex.Verify(x => x.GetUsersFromTicker(It.IsAny<string>()), Times.Never);
        _producer.Verify(x => x.PublishAsync(It.IsAny<PnLUpdate>()), Times.Never);
    }

    [Fact]
    public async Task HandlePriceUpdate_OldPriceNull_DoesNothing()
    {
        _priceStore
            .Setup(x => x.UpdateIfChanged("AAPL", 100))
            .ReturnsAsync((true, (decimal?)null));

        await _service.HandlePriceUpdate("AAPL", 100);

        _tickerIndex.Verify(x => x.GetUsersFromTicker(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task HandlePriceUpdate_ValidFlow_UpdatesAndPublishes()
    {
        var ticker = "AAPL";
        var user = "user1";

        _priceStore
            .Setup(x => x.UpdateIfChanged(ticker, 110))
            .ReturnsAsync((true, 100m));

        _tickerIndex
            .Setup(x => x.GetUsersFromTicker(ticker))
            .ReturnsAsync(new HashSet<string> { user });

        _positions
            .Setup(x => x.Get(user, ticker))
            .ReturnsAsync(new Position
            {
                Quantity = 10,
                AvgEntryPrice = 90
            });

        _pnlStore
            .Setup(x => x.Get(user))
            .ReturnsAsync(200m);

        await _service.HandlePriceUpdate(ticker, 110);

        _pnlStore.Verify(x => x.ChangePnlWithDelta(user, 100m), Times.Once);

        _producer.Verify(x => x.PublishAsync(It.Is<PnLUpdate>(msg =>
            msg.User == user &&
            msg.Ticker == ticker &&
            msg.Price == 110 &&
            msg.PositionPnL == (110 - 90) * 10 && 
            msg.PositionDelta == 100 &&
            msg.TotalPnL == 200 &&
            msg.TotalDelta == 100
        )), Times.Once);
    }

    [Fact]
    public async Task HandlePriceUpdate_NoPosition_SkipsUser()
    {
        var ticker = "AAPL";
        var user = "user1";

        _priceStore
            .Setup(x => x.UpdateIfChanged(ticker, 110))
            .ReturnsAsync((true, 100m));

        _tickerIndex
            .Setup(x => x.GetUsersFromTicker(ticker))
            .ReturnsAsync(new HashSet<string> { user });

        _positions
            .Setup(x => x.Get(user, ticker))
            .ReturnsAsync((Position)null);

        await _service.HandlePriceUpdate(ticker, 110);

        _pnlStore.Verify(x => x.ChangePnlWithDelta(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
        _producer.Verify(x => x.PublishAsync(It.IsAny<PnLUpdate>()), Times.Never);
    }

    [Fact]
    public async Task HandlePriceUpdate_MultipleUsers_ProcessesAll()
    {
        var ticker = "AAPL";

        var users = new HashSet<string> { "user1", "user2" };

        _priceStore
            .Setup(x => x.UpdateIfChanged(ticker, 120))
            .ReturnsAsync((true, 100m));

        _tickerIndex
            .Setup(x => x.GetUsersFromTicker(ticker))
            .ReturnsAsync(users);

        foreach (var user in users)
        {
            _positions
                .Setup(x => x.Get(user, ticker))
                .ReturnsAsync(new Position
                {
                    Quantity = 5,
                    AvgEntryPrice = 80
                });

            _pnlStore
                .Setup(x => x.Get(user))
                .ReturnsAsync(50m);
        }

        await _service.HandlePriceUpdate(ticker, 120);

        _pnlStore.Verify(x => x.ChangePnlWithDelta(It.IsAny<string>(), 100m), Times.Exactly(2));
        _producer.Verify(x => x.PublishAsync(It.IsAny<PnLUpdate>()), Times.Exactly(2));
    }
}