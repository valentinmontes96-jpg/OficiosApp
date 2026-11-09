using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("ZONAS")]
public class Zona
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdZona { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? CodigoPostal { get; set; }

    [Required]
    public bool Activa { get; set; } = true;

    // Propiedad de navegación
    public virtual ICollection<ProfesionalZona> ProfesionalesZonas { get; set; } = new List<ProfesionalZona>();
}