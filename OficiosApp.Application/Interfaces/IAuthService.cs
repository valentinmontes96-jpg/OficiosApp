using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Auth;
using OficiosApp.Shared.Dtos.Base;

namespace OficiosApp.Application.Interfaces;

public interface IAuthService
{
    Task<BaseResponse<AuthResponseDto>> RegistrarAsync(RegistroRequestDto dto);
    Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginRequestDto dto);
}