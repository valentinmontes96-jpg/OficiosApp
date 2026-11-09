using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Resenas;

namespace OficiosApp.Application.Interfaces;

public interface IResenaService
{
    Task<BaseResponse<IEnumerable<ResenaResponseDto>>> ObtenerTodasAsync();
    Task<BaseResponse<ResenaResponseDto>> ObtenerPorIdAsync(int idResena);
    Task<BaseResponse<ResenaResponseDto>> ObtenerPorCitaAsync(int idCita);
    Task<BaseResponse<IEnumerable<ResenaResponseDto>>> ObtenerPorProfesionalAsync(int idUsuarioProfesional);
    Task<BaseResponse<ResenaResponseDto>> CrearAsync(int idUsuarioCliente, CrearResenaDto dto);
    Task<BaseResponse<bool>> EliminarAsync(int idResena);
}