using OficiosApp.Shared.Dtos.Auth;
using OficiosApp.Shared.Dtos.Base;
using System.Net.Http.Json;
using OficiosApp.Shared.Dtos.Auth.Register.Request;
using OficiosApp.Shared.Dtos.Auth.Login.Request;

namespace OficiosApp.Web.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginDto)
    {
        var response = await _http.PostAsJsonAsync("api/Auth/login", loginDto);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var baseResponse = await response.Content.ReadFromJsonAsync<BaseResponse<AuthResponseDto>>();
        
        if (baseResponse != null && baseResponse.Success)
        {
            return baseResponse.Data;
        }

        return null;
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto registroDto)
    {
        var response = await _http.PostAsJsonAsync("api/Auth/registro", registroDto);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error al registrar: {errorBody}");
            return false;
        }

        return true;
    }
}