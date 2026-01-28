using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using VibePontaj.Data.Repositories;

namespace VibePontaj.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly IEmployeeRepository _employeeRepository;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage, IEmployeeRepository employeeRepository)
    {
        _sessionStorage = sessionStorage;
        _employeeRepository = employeeRepository;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult = await _sessionStorage.GetAsync<string>("UserEmail");
            var userEmail = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
            
            if (string.IsNullOrEmpty(userEmail))
            {
                return new AuthenticationState(_anonymous);
            }

            var user = await _employeeRepository.GetByEmailAsync(userEmail);
            if (user == null)
            {
                return new AuthenticationState(_anonymous);
            }

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }, "CustomAuth"));

            return new AuthenticationState(claimsPrincipal);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    public async Task UpdateAuthenticationState(string? userEmail)
    {
        ClaimsPrincipal claimsPrincipal;

        if (string.IsNullOrEmpty(userEmail))
        {
            await _sessionStorage.DeleteAsync("UserEmail");
            claimsPrincipal = _anonymous;
        }
        else
        {
            await _sessionStorage.SetAsync("UserEmail", userEmail);
            var user = await _employeeRepository.GetByEmailAsync(userEmail);
            
            if (user != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }, "CustomAuth"));
            }
            else
            {
                claimsPrincipal = _anonymous;
            }
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}
