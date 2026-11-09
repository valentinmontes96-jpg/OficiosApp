using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Citas.Response;

namespace OficiosApp.Application.Interfaces;

public interface IEstadoCitaService
{
    Task<BaseResponse<IEnumerable<EstadoCitaResponseDto>>> ObtenerTodosAsync();
    Task<BaseResponse<EstadoCitaResponseDto>> ObtenerPorIdAsync(int idEstadoCita);
}