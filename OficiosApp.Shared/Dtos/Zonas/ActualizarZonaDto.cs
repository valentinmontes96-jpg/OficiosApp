using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Zonas;

public class ActualizarZonaDto
{
    [Required(ErrorMessage = "El nombre de la zona es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(10)]
    public string? CodigoPostal { get; set; }

    public bool Activa { get; set; }
}