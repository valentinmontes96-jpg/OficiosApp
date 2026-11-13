using OficiosApp.Shared.Dtos.Citas;
using OficiosApp.Shared.Dtos.Citas.Response;

namespace OficiosApp.Mobile.Services;

public interface ICitaMobileService
{
    Task<List<CitaResponseDto>> ObtenerMisCitasProfesionalAsync();
    Task<List<CitaResponseDto>> ObtenerMisCitasClienteAsync();
    Task<(bool Exito, string Mensaje)> CrearCitaAsync(CrearCitaDto dto);
    Task<bool> CambiarEstadoCitaAsync(int idCita, int nuevoEstadoId, string? observaciones = null);
    Task<List<HistorialEstadoCitaResponseDto>> ObtenerHistorialCitaAsync(int idCita);
}
