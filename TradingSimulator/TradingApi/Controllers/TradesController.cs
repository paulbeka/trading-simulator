using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using TradingApi.Contracts.Trading;
using TradingApi.Services.Interfaces;

namespace TradingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TradesController : ControllerBase
    {
        private readonly ITradingService _tradingService;

        public TradesController(ITradingService tradingService)
        {
            _tradingService = tradingService;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy([FromBody] TradeRequest request)
        {
            try
            {
                var userId = GetUserId();

                request.Side = "Buy";

                var result = await _tradingService.ExecuteTradeAsync(userId, request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sell")]
        public async Task<IActionResult> Sell([FromBody] TradeRequest request)
        {
            try
            {
                var userId = GetUserId();

                request.Side = "Sell";

                var result = await _tradingService.ExecuteTradeAsync(userId, request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Trade([FromBody] TradeRequest request)
        {
            try
            {
                var userId = GetUserId();

                var result = await _tradingService.ExecuteTradeAsync(userId, request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID claim not found.");

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException($"Invalid user ID format: {userIdClaim.Value}");

            return userId;
        }
    }
}