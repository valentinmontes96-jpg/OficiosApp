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
    private bool _cargandoTokenDesdeStorage = false;

    public CustomAuthStateProvider(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string?> GetTokenAsync()
    {
        // 1. Si ya lo tenemos en memoria, retornar INMEDIATAMENTE sin interop JS
        if (!string.IsNullOrWhiteSpace(_tokenActual))
            return _tokenActual;

        // 2. Si ya hay una lectura en curso, esperar un instante para no colisionar en JS
        if (_cargandoTokenDesdeStorage)
        {
            var intentos = 0;
            while (_cargandoTokenDesdeStorage && intentos < 20)
            {
                await Task.Delay(50);
                intentos++;
            }
            return _tokenActual;
        }

        try
        {
            _cargandoTokenDesdeStorage = true;
            var userSessionResult = await _localStorage.GetAsync<string>("authToken");
            if (userSessionResult.Success && !string.IsNullOrWhiteSpace(userSessionResult.Value))
            {
                _tokenActual = userSessionResult.Value;
                return _tokenActual;
            }
        }
        catch
        {
            // Falla esperada si aún no montó JS o hubo desconexión temporal
        }
        finally
        {
            _cargandoTokenDesdeStorage = false;
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
        _tokenActual = token; // Se guarda en memoria primero

        if (!string.IsNullOrWhiteSpace(token))
        {
            try
            {
                await _localStorage.SetAsync("authToken", token);
            }
            catch
            {
                // Ignorar error si se desconecta momentáneamente el interop
            }
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
        }
        else
        {
            try
            {
                await _localStorage.DeleteAsync("authToken");
            }
            catch { }
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs == null)
            return Enumerable.Empty<Claim>();

        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
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