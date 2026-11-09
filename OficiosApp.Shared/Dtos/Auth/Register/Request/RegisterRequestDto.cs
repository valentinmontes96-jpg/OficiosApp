using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Auth.Register.Request;

public class RegisterRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    public int IdRol { get; set; } // 1: Cliente, 2: Profesional
}