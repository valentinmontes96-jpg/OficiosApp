using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Portfolio;

public class CrearFotoPortfolioDto
{
    [Required(ErrorMessage = "La URL de la imagen es obligatoria.")]
    [MaxLength(255, ErrorMessage = "La URL no puede superar los 255 caracteres.")]
    public string ImagenUrl { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
    public string? Descripcion { get; set; }
}