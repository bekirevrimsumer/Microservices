using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel.Client;
using Microservices.App.Interfaces;
using Microservices.App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Shared.Dtos;

namespace Microservices.App.Services;

public class IdentityService : IIdentityService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _accessor;
    private readonly ServiceApiSettings _serviceApiSettings;
    private readonly ClientSettings _clientSettings;
    
    public IdentityService(HttpClient client, IHttpContextAccessor accessor, IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings)
    {
        _httpClient = client;
        _accessor = accessor;
        _serviceApiSettings = serviceApiSettings.Value;
        _clientSettings = clientSettings.Value;
    }
    
    public async Task<Response<bool>> SignIn(SignInViewModel signInViewModel)
    {
        var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest{Address = _serviceApiSettings.BaseUri, Policy = new DiscoveryPolicy{RequireHttps = false}});
        if (discovery.IsError)
        {
            throw new Exception(discovery.Error);
        }
        
        var passwordTokenRequest = new PasswordTokenRequest
        {
            Address = discovery.TokenEndpoint,
            ClientId = _clientSettings.WebClientForUser.ClientId,
            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
            UserName = signInViewModel.Email,
            Password = signInViewModel.Password
        };
        
        var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
        if (token.IsError)
        {
            var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
            var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            return Response<bool>.Error(errorDto.Errors, 404);
        }

        var userInfoRequest = new UserInfoRequest {Address = discovery.UserInfoEndpoint, Token = token.AccessToken};
        var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

        if (userInfo.IsError)
        {
            throw userInfo.Exception;
        }
        
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authenticationProperties = new AuthenticationProperties();
        authenticationProperties.StoreTokens(new List<AuthenticationToken>
        {
            new AuthenticationToken {Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken},
            new AuthenticationToken {Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken},
            new AuthenticationToken {Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)}
        });
        authenticationProperties.IsPersistent = signInViewModel.RememberMe;
        
        await _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

        return  Response<bool>.Success(200);
    }

    public Task<TokenResponse> GetAccessTokenByRefreshToken()
    {
        throw new NotImplementedException();
    }

    public Task RevokeRefreshToken()
    {
        throw new NotImplementedException();
    }
}