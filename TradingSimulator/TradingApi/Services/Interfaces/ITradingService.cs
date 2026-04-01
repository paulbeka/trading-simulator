using TradingApi.Contracts.Trading;

namespace TradingApi.Services.Interfaces
{
    public interface ITradingService
    {
        Task<TradeResponse> ExecuteTradeAsync(Guid userId, TradeRequest request);

        Task<List<PositionResponse>> GetUserPositionsAsync(Guid userId);

        Task<decimal> GetUserAccountBalance(Guid userId);
    }
}
