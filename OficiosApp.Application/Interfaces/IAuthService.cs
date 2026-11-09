using OficiosApp.Shared.Dtos.Auth.Login.Request;
using OficiosApp.Shared.Dtos.Auth.Login.Response;
using OficiosApp.Shared.Dtos.Auth.Register.Request;
using OficiosApp.Shared.Dtos.Auth;
using OficiosApp.Shared.Wrappers;

namespace OficiosApp.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    Task<ApiResponse<string>> RegisterAsync(RegisterRequestDto request);
}