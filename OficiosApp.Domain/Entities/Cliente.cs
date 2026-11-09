using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("CLIENTES")]
public class Cliente
{
    [Key]
    public int IdUsuario { get; set; } // PK y FK al mismo tiempo

    [MaxLength(200)]
    public string? DireccionReferencia { get; set; }

    // Propiedades de navegación
    [ForeignKey(nameof(IdUsuario))]
    public virtual Usuario Usuario { get; set; } = null!;
}