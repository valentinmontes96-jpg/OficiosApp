namespace OficiosApp.Shared.Dtos.Citas;

public class HistorialEstadoCitaResponseDto
{
    public int IdHistorial { get; set; }
    public int IdCita { get; set; }
    public int IdEstadoCita { get; set; }
    public string NombreEstado { get; set; } = string.Empty;
    public int IdUsuarioAccion { get; set; }
    public string NombreUsuarioAccion { get; set; } = string.Empty;
    public DateTime FechaCambio { get; set; }
    public string? Observaciones { get; set; }
}