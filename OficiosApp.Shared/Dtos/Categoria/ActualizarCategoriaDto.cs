using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Categoria;

public class ActualizarCategoriaDto
{
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [MaxLength(80, ErrorMessage = "El nombre no puede superar los 80 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
    public string? Descripcion { get; set; }

    [MaxLength(255, ErrorMessage = "La URL del ícono no puede superar los 255 caracteres.")]
    public string? IconoUrl { get; set; }

    public bool Activa { get; set; }
}