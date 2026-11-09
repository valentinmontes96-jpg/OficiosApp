namespace OficiosApp.Shared.Dtos.Citas.Response;

public class EstadoCitaResponseDto
{
    public int IdEstadoCita { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}