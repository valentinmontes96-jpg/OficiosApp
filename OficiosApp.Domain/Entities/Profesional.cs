using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("PROFESIONALES")]
public class Profesional
{
    [Key]
    public int IdUsuario { get; set; } // PK y FK al mismo tiempo

    public string? Biografia { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? TarifaBase { get; set; }

    [Required]
    public bool DisponibleUrgencia { get; set; } = false;

    public DateTime? UltimaActualizacionDisponibilidad { get; set; }

    // Propiedades de navegación
    [ForeignKey(nameof(IdUsuario))]
    public virtual Usuario Usuario { get; set; } = null!;

    public virtual ICollection<ProfesionalCategoria> ProfesionalesCategorias { get; set; } = new List<ProfesionalCategoria>();
    public virtual ICollection<ProfesionalZona> ProfesionalesZonas { get; set; } = new List<ProfesionalZona>();
}