using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Auth;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Auth.Register.Request;
using OficiosApp.Shared.Dtos.Auth.Login.Request;

namespace OficiosApp.Application.Interfaces;

public interface IAuthService
{
    Task<BaseResponse<AuthResponseDto>> RegistrarAsync(RegisterRequestDto dto);
    Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginRequestDto dto);
}