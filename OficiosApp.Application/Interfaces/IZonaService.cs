using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Zonas;

namespace OficiosApp.Application.Interfaces;

public interface IZonaService
{
    Task<BaseResponse<IEnumerable<ZonaResponseDto>>> ObtenerTodasAsync(bool soloActivas = true);
    Task<BaseResponse<ZonaResponseDto>> ObtenerPorIdAsync(int idZona);
    Task<BaseResponse<ZonaResponseDto>> CrearAsync(CrearZonaDto dto);
    Task<BaseResponse<ZonaResponseDto>> ActualizarAsync(int idZona, ActualizarZonaDto dto);
    Task<BaseResponse<bool>> EliminarAsync(int idZona); // Borrado lógico
}