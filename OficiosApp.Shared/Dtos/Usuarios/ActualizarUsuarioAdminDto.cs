using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Usuarios;

public class ActualizarUsuarioAdminDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [MaxLength(25)]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    public int IdRol { get; set; }

    [Required]
    public bool Activo { get; set; }
}