using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Citas;

public class CambiarEstadoCitaDto
{
    [Required]
    public int NuevoEstadoId { get; set; }

    [MaxLength(255, ErrorMessage = "Las observaciones no pueden superar los 255 caracteres.")]
    public string? Observaciones { get; set; }
}