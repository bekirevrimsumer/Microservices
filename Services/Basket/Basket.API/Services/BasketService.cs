using System.Text.Json;
using Basket.API.Dtos;
using Basket.API.Interfaces;
using Shared.Dtos;

namespace Basket.API.Services;

public class BasketService : IBasketService
{
    private readonly RedisService _redisService;
    
    public BasketService(RedisService redisService)
    {
        _redisService = redisService;
    }
    
    public async Task<Response<BasketDto>> GetBasket(string userId)
    {
        var existBasket = await _redisService.GetDatabase().StringGetAsync(userId);
        
        return string.IsNullOrEmpty(existBasket) ? Response<BasketDto>.Error("Basket not found", 404) 
            : Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200);
    }

    public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
    {
        var status = await _redisService.GetDatabase().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));
        
        return status ? Response<bool>.Success(true, 200) : Response<bool>.Error("Basket could not update or save", 500);
    }

    public async Task<Response<bool>> Delete(string userId)
    {
        var status = await _redisService.GetDatabase().KeyDeleteAsync(userId);
        
        return status ? Response<bool>.Success(true, 200) : Response<bool>.Error("Basket not found", 404);
    }
}