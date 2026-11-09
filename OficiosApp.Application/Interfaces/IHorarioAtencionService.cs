using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.HorariosAtencion;

namespace OficiosApp.Application.Interfaces;

public interface IHorarioAtencionService
{
    Task<BaseResponse<IEnumerable<HorarioAtencionResponseDto>>> ObtenerPorProfesionalAsync(int idUsuarioProfesional);
    Task<BaseResponse<HorarioAtencionResponseDto>> GuardarDiaAsync(int idUsuarioProfesional, CrearOActualizarHorarioDto dto);
    Task<BaseResponse<bool>> SincronizarSemanaAsync(int idUsuarioProfesional, List<CrearOActualizarHorarioDto> listaHorarios);
    Task<BaseResponse<bool>> EliminarAsync(int idHorario, int idUsuarioProfesional);
}