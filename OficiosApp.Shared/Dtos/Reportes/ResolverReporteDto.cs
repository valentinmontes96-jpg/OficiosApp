using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Reportes;

public class ResolverReporteDto
{
    [Required(ErrorMessage = "El estado de resolución es obligatorio.")]
    [MaxLength(20)]
    public string Estado { get; set; } = string.Empty; // "Resuelto", "Desestimado"
}