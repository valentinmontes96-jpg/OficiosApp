using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("ESTADOS_CITA")]
public class EstadoCita
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdEstadoCita { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Descripcion { get; set; }

    // Propiedades de navegación
    public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    public virtual ICollection<HistorialEstadoCita> Historiales { get; set; } = new List<HistorialEstadoCita>();
}