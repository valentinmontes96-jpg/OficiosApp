using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OficiosApp.Domain.Entities;

[Table("CATEGORIAS")]
public class Categoria
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCategoria { get; set; }

    [Required]
    [MaxLength(80)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Descripcion { get; set; }

    [MaxLength(255)]
    public string? IconoUrl { get; set; }

    [Required]
    public bool Activa { get; set; } = true;

    // Propiedad de navegación
    public virtual ICollection<ProfesionalCategoria> ProfesionalesCategorias { get; set; } = new List<ProfesionalCategoria>();
}