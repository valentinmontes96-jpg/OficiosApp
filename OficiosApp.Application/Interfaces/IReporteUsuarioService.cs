using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Reportes;

namespace OficiosApp.Application.Interfaces;

public interface IReporteUsuarioService
{
    Task<BaseResponse<IEnumerable<ReporteUsuarioResponseDto>>> ObtenerTodosAsync(string? estado = null);
    Task<BaseResponse<ReporteUsuarioResponseDto>> ObtenerPorIdAsync(int idReporte);
    Task<BaseResponse<ReporteUsuarioResponseDto>> CrearAsync(int idUsuarioDenunciante, CrearReporteDto dto);
    Task<BaseResponse<bool>> ResolverAsync(int idReporte, int idAdministrador, ResolverReporteDto dto);
}