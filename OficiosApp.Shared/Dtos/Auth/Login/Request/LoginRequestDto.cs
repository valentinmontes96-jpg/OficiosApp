using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Auth.Login.Request;

public class LoginRequestDto
{
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = string.Empty;
}