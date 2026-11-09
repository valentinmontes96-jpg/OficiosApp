using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("FOTOS_PORTFOLIO")]
public class FotoPortfolio
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdFoto { get; set; }

    [Required]
    public int IdUsuarioProfesional { get; set; }

    [Required]
    [MaxLength(255)]
    public string ImagenUrl { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Descripcion { get; set; }

    [Required]
    public DateTime FechaCarga { get; set; } = DateTime.UtcNow;

    // Propiedad de navegación
    [ForeignKey(nameof(IdUsuarioProfesional))]
    public virtual Profesional Profesional { get; set; } = null!;
}