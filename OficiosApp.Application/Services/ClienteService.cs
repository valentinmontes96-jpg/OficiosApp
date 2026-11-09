using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Clientes;

namespace OficiosApp.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClienteService> _logger;

    public ClienteService(IUnitOfWork unitOfWork, ILogger<ClienteService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<ClienteResponseDto>> ObtenerPorIdAsync(int idUsuario)
    {
        var cliente = await _unitOfWork.Clientes.ObtenerPorIdAsync(idUsuario);

        if (cliente == null)
            return new BaseResponse<ClienteResponseDto> { Success = false, ErrorMessage = "Cliente no encontrado." };

        return new BaseResponse<ClienteResponseDto> { Success = true, Data = MapearADto(cliente) };
    }

    public async Task<BaseResponse<ClienteResponseDto>> ActualizarPerfilAsync(int idUsuario, ActualizarClienteDto dto)
    {
        var cliente = await _unitOfWork.Clientes.ObtenerPorIdAsync(idUsuario);
        bool esNuevo = false;

        if (cliente == null)
        {
            cliente = new Cliente { IdUsuario = idUsuario };
            esNuevo = true;
        }

        cliente.DireccionReferencia = dto.DireccionReferencia;

        if (esNuevo)
        {
            await _unitOfWork.Clientes.AgregarAsync(cliente);
            _logger.LogInformation("Se creó el perfil de cliente para el usuario ID: {IdUsuario}", idUsuario);
        }
        else
        {
            _unitOfWork.Clientes.Actualizar(cliente);
        }

        await _unitOfWork.GuardarCambiosAsync();

        return new BaseResponse<ClienteResponseDto> { Success = true, Data = MapearADto(cliente) };
    }

    private static ClienteResponseDto MapearADto(Cliente c)
    {
        return new ClienteResponseDto
        {
            IdUsuario = c.IdUsuario,
            Nombre = c.Usuario?.Nombre ?? string.Empty,
            Apellido = c.Usuario?.Apellido ?? string.Empty,
            Email = c.Usuario?.Email ?? string.Empty,
            Telefono = c.Usuario?.Telefono ?? string.Empty,
            DireccionReferencia = c.DireccionReferencia
        };
    }
}