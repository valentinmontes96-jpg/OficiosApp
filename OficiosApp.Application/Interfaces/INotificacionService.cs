using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Notificaciones;

namespace OficiosApp.Application.Interfaces;

public interface INotificacionService
{
    Task<BaseResponse<IEnumerable<NotificacionResponseDto>>> ObtenerPorUsuarioAsync(int idUsuario, bool soloNoLeidas = false);
    Task<BaseResponse<int>> ContarNoLeidasAsync(int idUsuario);
    Task<BaseResponse<NotificacionResponseDto>> CrearAsync(CrearNotificacionDto dto);
    Task<BaseResponse<bool>> MarcarComoLeidaAsync(int idNotificacion, int idUsuario);
    Task<BaseResponse<bool>> MarcarTodasComoLeidasAsync(int idUsuario);
    Task<BaseResponse<bool>> EliminarAsync(int idNotificacion, int idUsuario);
}