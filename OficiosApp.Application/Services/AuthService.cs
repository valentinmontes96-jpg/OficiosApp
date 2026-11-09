using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos.Auth.Login.Request;
using OficiosApp.Shared.Dtos.Auth.Login.Response;
using OficiosApp.Shared.Dtos.Auth.Register.Request;
using OficiosApp.Shared.Dtos.Usuarios;
using OficiosApp.Shared.Dtos.Auth;
using OficiosApp.Shared.Wrappers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OficiosApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        // Actualizado a BuscarAsync
        var usuarios = await _unitOfWork.Usuarios.BuscarAsync(u => u.Email == request.Email);
        var usuario = usuarios.FirstOrDefault();

        if (usuario == null || !usuario.Activo)
            return new ApiResponse<LoginResponseDto>("Credenciales inválidas o cuenta inactiva.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            return new ApiResponse<LoginResponseDto>("Credenciales inválidas.");

        // Actualizado a ObtenerPorIdAsync
        var rol = await _unitOfWork.Categorias.ObtenerPorIdAsync(usuario.IdRol);
        var nombreRol = usuario.IdRol == 1 ? "Cliente" : "Profesional";

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] ?? "SuperClaveSecretaDeDesarrollo12345!");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, nombreRol)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var response = new LoginResponseDto
        {
            Token = tokenHandler.WriteToken(token),
            Usuario = new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                Rol = nombreRol
            }
        };

        return new ApiResponse<LoginResponseDto>(response, "Login exitoso.");
    }

    public async Task<ApiResponse<string>> RegisterAsync(RegisterRequestDto request)
    {
        // Actualizado a BuscarAsync
        var existe = await _unitOfWork.Usuarios.BuscarAsync(u => u.Email == request.Email);
        if (existe.Any())
            return new ApiResponse<string>("El correo electrónico ya está registrado.");

        var nuevoUsuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Email = request.Email,
            Telefono = request.Telefono,
            IdRol = request.IdRol,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Activo = true,
            FechaRegistro = DateTime.UtcNow
        };

        // Actualizado a AgregarAsync y GuardarCambiosAsync
        await _unitOfWork.Usuarios.AgregarAsync(nuevoUsuario);
        await _unitOfWork.GuardarCambiosAsync();

        if (request.IdRol == 1)
        {
            await _unitOfWork.Clientes.AgregarAsync(new Cliente { IdUsuario = nuevoUsuario.IdUsuario });
        }
        else if (request.IdRol == 2)
        {
            await _unitOfWork.Profesionales.AgregarAsync(new Profesional { IdUsuario = nuevoUsuario.IdUsuario });
        }

        // Actualizado a GuardarCambiosAsync
        await _unitOfWork.GuardarCambiosAsync();

        return new ApiResponse<string>("Usuario registrado correctamente.", "Registro exitoso.");
    }
}