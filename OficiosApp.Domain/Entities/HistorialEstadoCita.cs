using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("HISTORIAL_ESTADOS_CITA")]
public class HistorialEstadoCita
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdHistorial { get; set; }

    [Required]
    public int IdCita { get; set; }

    [Required]
    public int IdEstadoCita { get; set; }

    [Required]
    public int IdUsuarioAccion { get; set; }

    [Required]
    public DateTime FechaCambio { get; set; } = DateTime.UtcNow;

    [MaxLength(255)]
    public string? Observaciones { get; set; }

    // Propiedades de navegación
    [ForeignKey(nameof(IdCita))]
    public virtual Cita Cita { get; set; } = null!;

    [ForeignKey(nameof(IdEstadoCita))]
    public virtual EstadoCita Estado { get; set; } = null!;

    [ForeignKey(nameof(IdUsuarioAccion))]
    public virtual Usuario UsuarioAccion { get; set; } = null!;
}