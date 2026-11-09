using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("NOTIFICACIONES")]
public class Notificacion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdNotificacion { get; set; }

    [Required]
    public int IdUsuarioDestinatario { get; set; }

    [Required]
    [MaxLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Mensaje { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; } = string.Empty;

    public int? ReferenciaId { get; set; }

    [Required]
    public bool Leida { get; set; } = false;

    [Required]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // Propiedad de navegación
    [ForeignKey(nameof(IdUsuarioDestinatario))]
    public virtual Usuario Destinatario { get; set; } = null!;
}