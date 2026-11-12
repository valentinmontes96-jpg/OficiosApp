using OficiosApp.Shared.Dtos.Citas.Response;

namespace OficiosApp.Mobile.Services;

public interface ICitaMobileService
{
    Task<List<CitaResponseDto>> ObtenerMisCitasProfesionalAsync();
    Task<List<CitaResponseDto>> ObtenerMisCitasClienteAsync();
    Task<bool> CambiarEstadoCitaAsync(int idCita, int nuevoEstadoId, string? observaciones = null);
}

