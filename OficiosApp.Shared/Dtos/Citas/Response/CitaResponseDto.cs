namespace OficiosApp.Shared.Dtos.Citas.Response;

public class CitaResponseDto
{
    public int IdCita { get; set; }
    public int IdUsuarioCliente { get; set; }
    public int IdUsuarioProfesional { get; set; }
    public int IdEstadoCita { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public DateTime FechaServicioPactada { get; set; }
    public string DescripcionProblema { get; set; } = string.Empty;
    public string DireccionServicio { get; set; } = string.Empty;
    public DateTime? FechaRespuesta { get; set; }
}