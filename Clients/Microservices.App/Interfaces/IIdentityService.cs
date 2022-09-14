using IdentityModel.Client;
using Microservices.App.Models;
using Shared.Dtos;

namespace Microservices.App.Interfaces;

public interface IIdentityService
{
    Task<Response<bool>> SignIn(SignInViewModel signInViewModel);
    Task<TokenResponse> GetAccessTokenByRefreshToken();
    Task RevokeRefreshToken();
}