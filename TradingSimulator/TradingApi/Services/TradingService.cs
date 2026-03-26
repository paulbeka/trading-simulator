using Database.DbContext;
using Database.Entities;
using Database.Enums;
using Microsoft.EntityFrameworkCore;
using TradingApi.Contracts.Trading;
using TradingApi.Services.Interfaces;
using TradingApi.Services.StoreInterfaces;

namespace TradingApi.Services
{
    public class TradingService : ITradingService
    {
        private readonly TradingDbContext _dbContext;
        private readonly IPriceStore _priceStore;
        public TradingService(TradingDbContext context, IPriceStore priceStore)
        {
            _dbContext = context;
            _priceStore = priceStore;
        }

        public async Task<TradeResponse> ExecuteTradeAsync(Guid userId, TradeRequest request)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (account == null)
                throw new Exception("Account not found");

            var position = await _dbContext.Positions
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Symbol == request.Ticker);

            var marketPrice = await _priceStore.GetPrice(request.Ticker);

            if (marketPrice == null)
                throw new Exception($"No market price available for {request.Ticker}");

            decimal executionPrice = marketPrice.Value;
            decimal total = executionPrice * request.Quantity;
            var side = Enum.Parse<TradeSide>(request.Side, ignoreCase: true);

            if (side == TradeSide.Buy)
            {
                if (account.CashBalance < total)
                    throw new Exception("Insufficient funds");

                account.CashBalance -= total;

                if (position == null)
                {
                    position = new Position
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        Symbol = request.Ticker,
                        Quantity = request.Quantity,
                        AvgEntryPrice = executionPrice,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _dbContext.Positions.Add(position);
                }
                else
                {
                    var newQty = position.Quantity + request.Quantity;

                    position.AvgEntryPrice =
                        ((position.Quantity * position.AvgEntryPrice) + total) / newQty;

                    position.Quantity = newQty;
                    position.UpdatedAt = DateTime.UtcNow;
                }
            }
            else if (side == TradeSide.Sell)
            {
                if (position == null || position.Quantity < request.Quantity)
                    throw new Exception("Not enough shares");

                account.CashBalance += total;

                position.Quantity -= request.Quantity;
                position.UpdatedAt = DateTime.UtcNow;

                if (position.Quantity == 0)
                    _dbContext.Positions.Remove(position);
            }
            else
            {
                throw new Exception("Invalid trade side");
            }

            var trade = new Trade
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Ticker = request.Ticker,
                Quantity = request.Quantity,
                Price = executionPrice,
                Side = Enum.Parse<TradeSide>(request.Side, ignoreCase: true),
                Timestamp = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Trades.Add(trade);

            account.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return new TradeResponse
            {
                Ticker = request.Ticker,
                Quantity = request.Quantity,
                Price = executionPrice,
                Side = request.Side,
                RemainingCash = account.CashBalance
            };
        }

        public async Task<List<PositionResponse>> GetUserPositionsAsync(Guid userId)
        {
            var positions = await _dbContext.Positions
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return positions.Select(p => new PositionResponse
            {
                Ticker = p.Symbol,
                Quantity = p.Quantity,
                AvgEntryPrice = p.AvgEntryPrice,
                UpdatedAt = p.UpdatedAt
            }).ToList();
        }
    }
}