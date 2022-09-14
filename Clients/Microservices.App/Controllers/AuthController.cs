using Microservices.App.Interfaces;
using Microservices.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.App.Controllers;

public class AuthController : Controller
{
    private readonly IIdentityService _identityService;
    
    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public IActionResult Signin()
    { 
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Signin(SignInViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        var result = await _identityService.SignIn(model);
        if (!result.IsSuccess)
        {
            foreach (var resultError in result.Errors)
            {
                ModelState.AddModelError(String.Empty, resultError);
            }

            return View();
        }
        
        return RedirectToAction(nameof(Index), "Home");
    }
}