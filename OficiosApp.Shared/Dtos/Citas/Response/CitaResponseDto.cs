namespace OficiosApp.Shared.Dtos.Citas.Response;

public class CitaResponseDto
{
    public int IdCita { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public string ProfesionalNombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public DateTime FechaServicioPactada { get; set; }
    public string DescripcionProblema { get; set; } = string.Empty;
    public string DireccionServicio { get; set; } = string.Empty;

    // Solo se expone si el estado es 'Aceptada'
    public string? TelefonoContacto { get; set; }
}