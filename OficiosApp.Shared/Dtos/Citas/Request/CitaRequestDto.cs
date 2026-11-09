using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.Citas.Request;

public class CitaRequestDto
{
    [Required]
    public int IdUsuarioProfesional { get; set; }

    [Required]
    public DateTime FechaServicioPactada { get; set; }

    [Required]
    [MaxLength(500)]
    public string DescripcionProblema { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string DireccionServicio { get; set; } = string.Empty;
}