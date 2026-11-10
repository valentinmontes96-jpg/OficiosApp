using OficiosApp.Shared.Dtos.Zonas;

namespace OficiosApp.Web.Services;

public interface IZonaService
{
    Task<List<ZonaResponseDto>> ObtenerTodasAsync(bool soloActivas = false);
    Task<bool> CrearAsync(CrearZonaDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarZonaDto dto);
    Task<bool> EliminarAsync(int id);
}