using Microsoft.AspNetCore.Mvc;
using Shared.BaseController;
using Shared.Dtos;

namespace FakePayment.API.Controllers;

public class FakePaymentsController : BaseController
{
    [HttpPost]
    public IActionResult ReceivePayment()
    {
        return CreateActionResultInstance<NoContent>(Response<NoContent>.Success(200));
    }
}   
