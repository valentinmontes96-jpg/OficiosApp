using OficiosApp.Shared.Dtos.Usuarios;

namespace OficiosApp.Shared.Dtos.Auth.Login.Response;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UsuarioDto Usuario { get; set; } = null!;
}