using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Auth.Register.Request;
using OficiosApp.Shared.Dtos.Auth;
using OficiosApp.Shared.Dtos.Auth.Login.Request;
using OficiosApp.Shared.Dtos.Base;

namespace OficiosApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        ILogger<AuthService> logger)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<BaseResponse<AuthResponseDto>> RegistrarAsync(RegisterRequestDto dto)
    {
        var usuarios = await _unitOfWork.Usuarios.ObtenerTodosAsync();
        var existeEmail = usuarios.Any(u => u.Email.ToLower() == dto.Email.Trim().ToLower());

        if (existeEmail)
        {
            _logger.LogWarning("Intento de registro con email ya existente: {Email}", dto.Email);
            return new BaseResponse<AuthResponseDto>
            {
                Success = false,
                ErrorMessage = "El correo electrónico ya se encuentra registrado."
            };
        }

        var nuevoUsuario = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Apellido = dto.Apellido.Trim(),
            Email = dto.Email.Trim().ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Telefono = dto.Telefono.Trim(),
            IdRol = dto.IdRol,
            Activo = true,
            FechaRegistro = DateTime.UtcNow
        };

        await _unitOfWork.Usuarios.AgregarAsync(nuevoUsuario);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Usuario registrado exitosamente con ID {IdUsuario} y rol {IdRol}", nuevoUsuario.IdUsuario, nuevoUsuario.IdRol);

        var token = _tokenService.GenerarToken(nuevoUsuario);

        return new BaseResponse<AuthResponseDto>
        {
            Success = true,
            Data = new AuthResponseDto
            {
                IdUsuario = nuevoUsuario.IdUsuario,
                Nombre = nuevoUsuario.Nombre,
                Apellido = nuevoUsuario.Apellido,
                Email = nuevoUsuario.Email,
                IdRol = nuevoUsuario.IdRol,
                Token = token
            }
        };
    }

    public async Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        var usuarios = await _unitOfWork.Usuarios.ObtenerTodosAsync();
        var usuario = usuarios.FirstOrDefault(u => u.Email.ToLower() == dto.Email.Trim().ToLower());

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
        {
            _logger.LogWarning("Intento de inicio de sesión fallido para el email: {Email}", dto.Email);
            return new BaseResponse<AuthResponseDto>
            {
                Success = false,
                ErrorMessage = "Credenciales incorrectas."
            };
        }

        if (!usuario.Activo)
        {
            _logger.LogWarning("Intento de inicio de sesión en cuenta inactiva. Usuario ID: {IdUsuario}", usuario.IdUsuario);
            return new BaseResponse<AuthResponseDto>
            {
                Success = false,
                ErrorMessage = "La cuenta se encuentra inactiva."
            };
        }

        var token = _tokenService.GenerarToken(usuario);

        _logger.LogInformation("Inicio de sesión exitoso. Usuario ID: {IdUsuario}", usuario.IdUsuario);

        return new BaseResponse<AuthResponseDto>
        {
            Success = true,
            Data = new AuthResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                IdRol = usuario.IdRol,
                Token = token
            }
        };
    }
}