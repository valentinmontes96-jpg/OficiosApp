using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Clientes;

namespace OficiosApp.Application.Interfaces;

public interface IClienteService
{
    Task<BaseResponse<ClienteResponseDto>> ObtenerPorIdAsync(int idUsuario);
    Task<BaseResponse<ClienteResponseDto>> ActualizarPerfilAsync(int idUsuario, ActualizarClienteDto dto);
}