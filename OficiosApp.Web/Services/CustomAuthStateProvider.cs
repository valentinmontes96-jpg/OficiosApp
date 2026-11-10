using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace OficiosApp.Web.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private string? _tokenActual;

    public CustomAuthStateProvider(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string?> GetTokenAsync()
    {
        if (!string.IsNullOrWhiteSpace(_tokenActual))
            return _tokenActual;

        try
        {
            var userSessionResult = await _localStorage.GetAsync<string>("authToken");
            if (userSessionResult.Success && !string.IsNullOrWhiteSpace(userSessionResult.Value))
            {
                _tokenActual = userSessionResult.Value;
                return _tokenActual;
            }
        }
        catch
        {
            // Falla esperada si aún no montó JS
        }

        return null;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(_anonymous);

        try
        {
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
            return new AuthenticationState(claimsPrincipal);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    public async Task UpdateAuthenticationState(string? token)
    {
        ClaimsPrincipal claimsPrincipal;
        _tokenActual = token;

        if (!string.IsNullOrWhiteSpace(token))
        {
            await _localStorage.SetAsync("authToken", token);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
        }
        else
        {
            await _localStorage.DeleteAsync("authToken");
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}