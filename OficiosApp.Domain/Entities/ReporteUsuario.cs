using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("REPORTES_USUARIOS")]
public class ReporteUsuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdReporte { get; set; }

    [Required]
    public int IdUsuarioDenunciante { get; set; }

    [Required]
    public int IdUsuarioDenunciado { get; set; }

    [Required]
    [MaxLength(150)]
    public string Motivo { get; set; } = string.Empty;

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    public DateTime FechaReporte { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(20)]
    public string Estado { get; set; } = "Pendiente";

    public int? IdAdministradorResolutor { get; set; }

    // Propiedades de navegación
    [ForeignKey(nameof(IdUsuarioDenunciante))]
    public virtual Usuario Denunciante { get; set; } = null!;

    [ForeignKey(nameof(IdUsuarioDenunciado))]
    public virtual Usuario Denunciado { get; set; } = null!;

    [ForeignKey(nameof(IdAdministradorResolutor))]
    public virtual Usuario? AdministradorResolutor { get; set; }
}