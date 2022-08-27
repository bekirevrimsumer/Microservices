using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Commands;
using Order.Application.Queries;
using Shared.BaseController;
using Shared.Interfaces;

namespace Order.API.Controllers;

public class OrdersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ISharedIdentityService _sharedIdentityService;
    
    public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentityService)
    {
        _mediator = mediator;
        _sharedIdentityService = sharedIdentityService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var result = await _mediator.Send(new GetOrdersByUserIdQuery{UserId = _sharedIdentityService.GetUserId});
        return CreateActionResultInstance(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return CreateActionResultInstance(result);
    }
}