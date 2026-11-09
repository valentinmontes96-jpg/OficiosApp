using System.ComponentModel.DataAnnotations;

namespace OficiosApp.Shared.Dtos.HorariosAtencion;

public class CrearOActualizarHorarioDto
{
    [Required]
    [Range(0, 6, ErrorMessage = "El día debe estar entre 0 (Domingo) y 6 (Sábado).")]
    public byte DiaSemana { get; set; }

    [Required]
    public TimeSpan HoraApertura { get; set; }

    [Required]
    public TimeSpan HoraCierre { get; set; }
}