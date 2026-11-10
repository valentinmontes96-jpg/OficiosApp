using OficiosApp.Shared.Dtos.Auth.Login.Request;
using OficiosApp.Shared.Dtos.Auth.Login.Response;
using OficiosApp.Shared.Dtos.Auth.Register;
using OficiosApp.Shared.Dtos.Auth.Register.Request;
using OficiosApp.Shared.Dtos.Auth;
namespace OficiosApp.Web.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginDto);
    Task<bool> RegisterAsync(RegisterRequestDto registroDto);
}