using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("PROFESIONALES_CATEGORIAS")]
public class ProfesionalCategoria
{
    public int IdUsuarioProfesional { get; set; }
    public int IdCategoria { get; set; }

    // Propiedades de navegación
    [ForeignKey(nameof(IdUsuarioProfesional))]
    public virtual Profesional Profesional { get; set; } = null!;

    [ForeignKey(nameof(IdCategoria))]
    public virtual Categoria Categoria { get; set; } = null!;
}