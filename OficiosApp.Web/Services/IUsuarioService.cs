using OficiosApp.Shared.Dtos.Usuarios;

namespace OficiosApp.Web.Services;

public interface IUsuarioService
{
    Task<List<UsuarioDto>> ObtenerTodosAsync();
    Task<UsuarioDto?> ObtenerPorIdAsync(int id);
    Task<bool> ActualizarAsync(int id, ActualizarUsuarioAdminDto dto);
    Task<bool> EliminarAsync(int id);
}