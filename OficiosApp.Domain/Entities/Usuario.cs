using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("USUARIOS")]
public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUsuario { get; set; }

    [Required]
    public int IdRol { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(25)]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    public bool Activo { get; set; } = true;

    [Required]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Propiedades de navegación
    [ForeignKey(nameof(IdRol))]
    public virtual Rol Rol { get; set; } = null!;
}