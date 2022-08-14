using Basket.API.Dtos;
using Shared.Dtos;

namespace Basket.API.Interfaces;

public interface IBasketService
{
    Task<Response<BasketDto>> GetBasket(string userId);
    Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);
    Task<Response<bool>> Delete(string userId);
}