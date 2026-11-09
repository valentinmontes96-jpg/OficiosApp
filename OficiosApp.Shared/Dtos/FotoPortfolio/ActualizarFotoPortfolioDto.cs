using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Portfolio;

public class ActualizarFotoPortfolioDto
{
    [MaxLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
    public string? Descripcion { get; set; }
}