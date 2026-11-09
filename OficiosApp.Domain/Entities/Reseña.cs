using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("RESENAS")]
public class Resena
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdResena { get; set; }

    [Required]
    public int IdCita { get; set; }

    [Required]
    [Range(1, 5)]
    public byte Puntuacion { get; set; }

    public string? Comentario { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // Propiedad de navegación
    [ForeignKey(nameof(IdCita))]
    public virtual Cita Cita { get; set; } = null!;
}