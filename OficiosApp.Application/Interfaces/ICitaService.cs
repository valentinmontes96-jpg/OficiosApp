using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Citas;
using OficiosApp.Shared.Dtos.Citas.Response;


namespace OficiosApp.Application.Interfaces;

public interface ICitaService
{
    Task<BaseResponse<IEnumerable<CitaResponseDto>>> ObtenerTodasAsync();
    Task<BaseResponse<CitaResponseDto>> ObtenerPorIdAsync(int idCita);
    Task<BaseResponse<bool>> CambiarEstadoAsync(int idCita, int idUsuarioAccion, int nuevoIdEstado, string? observaciones = null);
    Task<BaseResponse<IEnumerable<HistorialEstadoCitaResponseDto>>> ObtenerHistorialAsync(int idCita);
    Task<BaseResponse<IEnumerable<CitaResponseDto>>> ObtenerPorClienteAsync(int idUsuarioCliente);
    Task<BaseResponse<IEnumerable<CitaResponseDto>>> ObtenerPorProfesionalAsync(int idUsuarioProfesional);
    Task<BaseResponse<CitaResponseDto>> CrearAsync(int idUsuarioCliente, CrearCitaDto dto, int idEstadoInicial = 1);
    Task<BaseResponse<CitaResponseDto>> ActualizarAsync(int idCita, ActualizarCitaDto dto);
}