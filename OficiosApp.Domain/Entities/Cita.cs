using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("CITAS")]
public class Cita
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCita { get; set; }

    [Required]
    public int IdUsuarioCliente { get; set; }

    [Required]
    public int IdUsuarioProfesional { get; set; }

    [Required]
    public int IdEstadoCita { get; set; }

    [Required]
    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime FechaServicioPactada { get; set; }

    [Required]
    public string DescripcionProblema { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string DireccionServicio { get; set; } = string.Empty;

    public DateTime? FechaRespuesta { get; set; }

    // Propiedades de navegación
    [ForeignKey(nameof(IdUsuarioCliente))]
    public virtual Cliente Cliente { get; set; } = null!;

    [ForeignKey(nameof(IdUsuarioProfesional))]
    public virtual Profesional Profesional { get; set; } = null!;

    [ForeignKey(nameof(IdEstadoCita))]
    public virtual EstadoCita Estado { get; set; } = null!;

    public virtual Resena? Resena { get; set; }
    public virtual ICollection<HistorialEstadoCita> Historial { get; set; } = new List<HistorialEstadoCita>();
}