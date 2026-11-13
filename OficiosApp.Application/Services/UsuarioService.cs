using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Perfil;
using OficiosApp.Shared.Dtos.Usuarios;

namespace OficiosApp.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;

    public UsuarioService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<PerfilResponseDto>> ObtenerMiPerfilAsync(int idUsuario)
    {
        return await ObtenerPorIdAsync(idUsuario);
    }

    public async Task<BaseResponse<PerfilResponseDto>> ActualizarMiPerfilAsync(int idUsuario, UpdatePerfilRequestDto request)
    {
        var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(idUsuario);

        if (usuario == null || !usuario.Activo)
            return new BaseResponse<PerfilResponseDto> { Success = false, ErrorMessage = "Usuario no encontrado." };

        usuario.Nombre = request.Nombre;
        usuario.Apellido = request.Apellido;
        usuario.Telefono = request.Telefono;

        _unitOfWork.Usuarios.Actualizar(usuario);
        await _unitOfWork.GuardarCambiosAsync();

        return await ObtenerPorIdAsync(idUsuario);
    }

    public async Task<BaseResponse<IEnumerable<PerfilResponseDto>>> ObtenerTodosAsync()
    {
        var usuarios = await _unitOfWork.Usuarios.ObtenerTodosAsync();

        var dtos = usuarios.Select(u => new PerfilResponseDto
        {
            IdUsuario = u.IdUsuario,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            Email = u.Email,
            Telefono = u.Telefono,
            IdRol = u.IdRol,
            Rol = u.Rol?.Nombre ?? ObtenerNombreRolPorId(u.IdRol),
            Activo = u.Activo,
            FechaRegistro = u.FechaRegistro
        }).ToList();

        return new BaseResponse<IEnumerable<PerfilResponseDto>>
        {
            Success = true,
            Data = dtos
        };
    }

    public async Task<BaseResponse<PerfilResponseDto>> ObtenerPorIdAsync(int idUsuario)
    {
        var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
            return new BaseResponse<PerfilResponseDto> { Success = false, ErrorMessage = "Usuario no encontrado." };

        var dto = new PerfilResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            Telefono = usuario.Telefono,
            IdRol = usuario.IdRol,
            Rol = usuario.Rol?.Nombre ?? ObtenerNombreRolPorId(usuario.IdRol),
            Activo = usuario.Activo,
            FechaRegistro = usuario.FechaRegistro
        };

        return new BaseResponse<PerfilResponseDto> { Success = true, Data = dto };
    }

    public async Task<BaseResponse<PerfilResponseDto>> ActualizarAsync(int idUsuario, ActualizarUsuarioAdminDto dto)
    {
        var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
            return new BaseResponse<PerfilResponseDto> { Success = false, ErrorMessage = "Usuario no encontrado." };

        usuario.Nombre = dto.Nombre;
        usuario.Apellido = dto.Apellido;
        usuario.Telefono = dto.Telefono;
        usuario.IdRol = dto.IdRol;
        usuario.Activo = dto.Activo;

        _unitOfWork.Usuarios.Actualizar(usuario);
        await _unitOfWork.GuardarCambiosAsync();

        return await ObtenerPorIdAsync(idUsuario);
    }

    public async Task<BaseResponse<bool>> EliminarAsync(int idUsuario)
    {
        var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Usuario no encontrado." };

        // Borrado lógico
        usuario.Activo = false;
        _unitOfWork.Usuarios.Actualizar(usuario);
        await _unitOfWork.GuardarCambiosAsync();

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    private static string ObtenerNombreRolPorId(int idRol) => idRol switch
    {
        1 => "Admin",
        2 => "Cliente",
        3 => "Profesional",
        _ => string.Empty
    };
}