using OficiosApp.Shared.Dtos.HorariosAtencion;
using OficiosApp.Shared.Dtos.Profesionales;
using OficiosApp.Shared.Dtos.Zonas;

namespace OficiosApp.Mobile.Services;

public interface IProfesionalMobileService
{
    Task<List<ProfesionalResponseDto>> ObtenerTodosAsync();
    Task<ProfesionalResponseDto?> ObtenerPerfilAsync(int idUsuario);
    Task<ProfesionalResponseDto?> ActualizarPerfilAsync(ActualizarProfesionalDto dto);
    Task<bool> AsignarCategoriasAsync(List<int> idsCategorias);
    Task<bool> AsignarZonasAsync(List<int> idsZonas);
    Task<List<ZonaResponseDto>> ObtenerZonasAsync();
    Task<List<HorarioAtencionResponseDto>> ObtenerHorariosAsync(int idProfesional);
    Task<bool> SincronizarHorariosAsync(List<CrearOActualizarHorarioDto> horarios);
}
