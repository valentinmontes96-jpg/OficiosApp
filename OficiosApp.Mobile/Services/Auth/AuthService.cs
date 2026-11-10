using OficiosApp.Shared.Dtos.Auth;
using OficiosApp.Shared.Dtos.Auth.Login.Request;
using OficiosApp.Shared.Dtos.Auth.Login.Response;
using OficiosApp.Shared.Dtos.Auth.Register.Request;
using OficiosApp.Shared.Dtos.Usuarios;
using System.Net.Http.Json;

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

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (result != null && !string.IsNullOrWhiteSpace(result.Token))
            {
                await _tokenStorage.SetTokenAsync(result.Token);
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto registerDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
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
}