using OficiosApp.Shared.Dtos.Auth.Login.Request; // O la ubicación de tus DTOs
using OficiosApp.Shared.Dtos.Auth.Register.Request;

namespace OficiosApp.Mobile.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginRequestDto loginDto);
    Task<(bool Exito, string Mensaje)> RegisterAsync(RegisterRequestDto registerDto);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<int?> ObtenerIdRolAsync();
    Task<int?> ObtenerIdUsuarioAsync();
    Task<string?> ObtenerNombreUsuarioAsync();
    Task<string> ObtenerRutaHomePorRolAsync();
}