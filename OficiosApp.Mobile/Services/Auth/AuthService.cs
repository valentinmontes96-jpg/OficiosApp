using OficiosApp.Shared.Dtos.Auth;
using OficiosApp.Shared.Dtos.Auth.Login.Request;
using OficiosApp.Shared.Dtos.Auth.Login.Response;
using OficiosApp.Shared.Dtos.Auth.Register.Request;
using OficiosApp.Shared.Dtos.Base;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace OficiosApp.Mobile.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenStorage _tokenStorage;

    public AuthService(HttpClient httpClient, ITokenStorage tokenStorage)
    {
        _httpClient = httpClient;
        _tokenStorage = tokenStorage;
    }

    public async Task<bool> LoginAsync(LoginRequestDto request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode)
                return false;

            // Desempaquetamos con tu BaseResponse
            var result = await response.Content.ReadFromJsonAsync<BaseResponse<AuthResponseDto>>();

            if (result != null && result.Success && result.Data != null && !string.IsNullOrWhiteSpace(result.Data.Token))
            {
                var nombreCompleto = $"{result.Data.Nombre} {result.Data.Apellido}".Trim();
                await _tokenStorage.SetTokenAsync(
                    result.Data.Token,
                    result.Data.IdRol > 0 ? result.Data.IdRol : null,
                    result.Data.IdUsuario > 0 ? result.Data.IdUsuario : null,
                    nombreCompleto);
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<(bool Exito, string Mensaje)> RegisterAsync(RegisterRequestDto registerDto)
    {
        try
        {
            // Cambiado a "api/auth/registro" para coincidir con el controller
            var response = await _httpClient.PostAsJsonAsync("api/auth/registro", registerDto);

            var rawContent = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(rawContent))
            {
                if (response.IsSuccessStatusCode)
                    return (true, string.Empty);

                return (false, $"El servidor respondió con código {(int)response.StatusCode} sin contenido.");
            }

            try
            {
                var result = JsonSerializer.Deserialize<BaseResponse<object>>(
                    rawContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (response.IsSuccessStatusCode && result != null && result.Success)
                {
                    return (true, string.Empty);
                }

                return (false, result?.ErrorMessage ?? $"Error en el registro (Código {(int)response.StatusCode}).");
            }
            catch
            {
                return (false, $"Error del servidor ({(int)response.StatusCode}).");
            }
        }
        catch (Exception ex)
        {
            return (false, "Error de conexión con el servidor: " + ex.Message);
        }
    }

    public Task LogoutAsync()
    {
        _tokenStorage.RemoveToken();
        return Task.CompletedTask;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _tokenStorage.GetTokenAsync();
        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task<int?> ObtenerIdRolAsync()
    {
        var cached = _tokenStorage.GetCachedIdRol();
        if (cached.HasValue && cached.Value > 0)
            return cached.Value;

        var valor = await ObtenerClaimDesdeTokenAsync("IdRol", "idRol", ClaimTypes.Role, "role");
        return int.TryParse(valor, out var idRol) ? idRol : null;
    }

    public async Task<int?> ObtenerIdUsuarioAsync()
    {
        var cached = _tokenStorage.GetCachedIdUsuario();
        if (cached.HasValue && cached.Value > 0)
            return cached.Value;

        var valor = await ObtenerClaimDesdeTokenAsync("nameid", ClaimTypes.NameIdentifier, "sub", "IdUsuario", "idUsuario");
        return int.TryParse(valor, out var idUsuario) ? idUsuario : null;
    }

    public async Task<string?> ObtenerNombreUsuarioAsync()
    {
        var cached = _tokenStorage.GetCachedNombreUsuario();
        if (!string.IsNullOrWhiteSpace(cached))
            return cached;

        return await ObtenerClaimDesdeTokenAsync("unique_name", ClaimTypes.Name, "name");
    }

    public async Task<string> ObtenerRutaHomePorRolAsync()
    {
        var idRol = await ObtenerIdRolAsync();
        return idRol == 3 ? "/home-profesional" : "/home-cliente";
    }

    private async Task<string?> ObtenerClaimDesdeTokenAsync(params string[] clavesPosibles)
    {
        var token = await _tokenStorage.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return null;

        try
        {
            var partes = token.Split('.');
            if (partes.Length < 2)
                return null;

            var payload = partes[1].Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var bytes = Convert.FromBase64String(payload);
            using var doc = JsonDocument.Parse(bytes);

            foreach (var prop in doc.RootElement.EnumerateObject())
            {
                foreach (var clave in clavesPosibles)
                {
                    if (string.Equals(prop.Name, clave, StringComparison.OrdinalIgnoreCase))
                    {
                        return prop.Value.ValueKind == JsonValueKind.Number
                            ? prop.Value.GetInt32().ToString()
                            : prop.Value.GetString();
                    }
                }
            }

            return null;
        }
        catch
        {
            return null;
        }
    }
}