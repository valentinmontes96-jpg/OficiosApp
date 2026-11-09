using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("FAVORITOS")]
public class Favorito
{
    public int IdUsuarioCliente { get; set; }
    public int IdUsuarioProfesional { get; set; }

    public DateTime FechaAgregado { get; set; } = DateTime.UtcNow;

    // Propiedades de navegación
    [ForeignKey(nameof(IdUsuarioCliente))]
    public virtual Cliente Cliente { get; set; } = null!;

    [ForeignKey(nameof(IdUsuarioProfesional))]
    public virtual Profesional Profesional { get; set; } = null!;
}