using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Profesionales;

namespace OficiosApp.Application.Interfaces;

public interface IProfesionalService
{
    Task<BaseResponse<IEnumerable<ProfesionalResponseDto>>> ObtenerTodosAsync();
    Task<BaseResponse<ProfesionalResponseDto>> ObtenerPorIdAsync(int idUsuario);
    Task<BaseResponse<ProfesionalResponseDto>> ActualizarPerfilAsync(int idUsuario, ActualizarProfesionalDto dto);

    // Gestión de tablas intermedias
    Task<BaseResponse<bool>> AsignarCategoriasAsync(int idUsuario, List<int> idsCategorias);
    Task<BaseResponse<bool>> AsignarZonasAsync(int idUsuario, List<int> idsZonas);
}