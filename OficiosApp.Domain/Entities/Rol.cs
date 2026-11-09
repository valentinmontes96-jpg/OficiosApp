using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("ROLES")]
public class Rol
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdRol { get; set; }

    [Required]
    [MaxLength(30)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Descripcion { get; set; }

    // Propiedad de navegación
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}