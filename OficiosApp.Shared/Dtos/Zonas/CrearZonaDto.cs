using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Zonas;

public class CrearZonaDto
{
    [Required(ErrorMessage = "El nombre de la zona es obligatorio")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(10, ErrorMessage = "El código postal no puede superar los 10 caracteres")]
    public string? CodigoPostal { get; set; }
}