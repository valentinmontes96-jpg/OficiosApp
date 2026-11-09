using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Perfil;
using OficiosApp.Shared.Dtos.Usuarios;
namespace OficiosApp.Application.Interfaces;

public interface IUsuarioService
{
    // Métodos para el usuario autenticado
    Task<BaseResponse<PerfilResponseDto>> ObtenerMiPerfilAsync(int idUsuario);
    Task<BaseResponse<PerfilResponseDto>> ActualizarMiPerfilAsync(int idUsuario, UpdatePerfilRequestDto request);

    // Métodos administrativos (CRUD completo)
    Task<BaseResponse<IEnumerable<PerfilResponseDto>>> ObtenerTodosAsync();
    Task<BaseResponse<PerfilResponseDto>> ObtenerPorIdAsync(int idUsuario);
    Task<BaseResponse<PerfilResponseDto>> ActualizarAsync(int idUsuario, ActualizarUsuarioAdminDto dto);
    Task<BaseResponse<bool>> EliminarAsync(int idUsuario); // Borrado lógico (Activo = false)
}