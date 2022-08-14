using Discount.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseController;
using Shared.Interfaces;

namespace Discount.API.Controllers;

public class DiscountsController : BaseController
{
    private readonly IDiscountService _discountService;
    private readonly ISharedIdentityService _sharedIdentityService;
    
    public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
    {
        _discountService = discountService;
        _sharedIdentityService = sharedIdentityService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
       return CreateActionResultInstance(await _discountService.GetAll());
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return CreateActionResultInstance(await _discountService.GetById(id));
    }
    
    [Route("/api/[controller]/[action]/{code}")]
    [HttpGet]
    public async Task<IActionResult> GetByCode(string code)
    {
        return CreateActionResultInstance(await _discountService.GetByCodeAndUserId(code, _sharedIdentityService.GetUserId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Models.Discount discount)
    {
        return CreateActionResultInstance(await _discountService.Create(discount));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(Models.Discount discount)
    {
        return CreateActionResultInstance(await _discountService.Update(discount));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return CreateActionResultInstance(await _discountService.Delete(id));
    }
}