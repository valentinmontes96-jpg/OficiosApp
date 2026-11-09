using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Roles;

namespace OficiosApp.Application.Interfaces;

public interface IRolService
{
    Task<BaseResponse<IEnumerable<RolResponseDto>>> ObtenerTodosAsync();
    Task<BaseResponse<RolResponseDto>> ObtenerPorIdAsync(int idRol);
}