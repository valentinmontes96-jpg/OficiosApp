using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("PROFESIONALES_ZONAS")]
public class ProfesionalZona
{
    public int IdUsuarioProfesional { get; set; }
    public int IdZona { get; set; }

    // Propiedades de navegación
    [ForeignKey(nameof(IdUsuarioProfesional))]
    public virtual Profesional Profesional { get; set; } = null!;

    [ForeignKey(nameof(IdZona))]
    public virtual Zona Zona { get; set; } = null!;
}