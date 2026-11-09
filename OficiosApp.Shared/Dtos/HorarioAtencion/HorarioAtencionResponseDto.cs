namespace OficiosApp.Shared.Dtos.HorariosAtencion;

public class HorarioAtencionResponseDto
{
    public int IdHorario { get; set; }
    public int IdUsuarioProfesional { get; set; }
    public byte DiaSemana { get; set; }
    public TimeSpan HoraApertura { get; set; }
    public TimeSpan HoraCierre { get; set; }
}