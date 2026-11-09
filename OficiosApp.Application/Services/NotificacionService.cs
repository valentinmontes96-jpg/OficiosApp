using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Notificaciones;

namespace OficiosApp.Application.Services;

public class NotificacionService : INotificacionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<NotificacionService> _logger;

    public NotificacionService(IUnitOfWork unitOfWork, ILogger<NotificacionService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<NotificacionResponseDto>>> ObtenerPorUsuarioAsync(int idUsuario, bool soloNoLeidas = false)
    {
        var notificaciones = await _unitOfWork.Notificaciones.ObtenerTodosAsync();

        var query = notificaciones.Where(n => n.IdUsuarioDestinatario == idUsuario);

        if (soloNoLeidas)
            query = query.Where(n => !n.Leida);

        var lista = query
            .OrderByDescending(n => n.FechaCreacion)
            .Select(MapearADto)
            .ToList();

        return new BaseResponse<IEnumerable<NotificacionResponseDto>>
        {
            Success = true,
            Data = lista
        };
    }

    public async Task<BaseResponse<int>> ContarNoLeidasAsync(int idUsuario)
    {
        var notificaciones = await _unitOfWork.Notificaciones.ObtenerTodosAsync();
        var cantidad = notificaciones.Count(n => n.IdUsuarioDestinatario == idUsuario && !n.Leida);

        return new BaseResponse<int> { Success = true, Data = cantidad };
    }

    public async Task<BaseResponse<NotificacionResponseDto>> CrearAsync(CrearNotificacionDto dto)
    {
        var nuevaNotificacion = new Notificacion
        {
            IdUsuarioDestinatario = dto.IdUsuarioDestinatario,
            Titulo = dto.Titulo.Trim(),
            Mensaje = dto.Mensaje.Trim(),
            Tipo = dto.Tipo.Trim(),
            ReferenciaId = dto.ReferenciaId,
            Leida = false,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Notificaciones.AgregarAsync(nuevaNotificacion);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Notificación {IdNotificacion} creada para el usuario {IdUsuario}", nuevaNotificacion.IdNotificacion, dto.IdUsuarioDestinatario);

        return new BaseResponse<NotificacionResponseDto> { Success = true, Data = MapearADto(nuevaNotificacion) };
    }

    public async Task<BaseResponse<bool>> MarcarComoLeidaAsync(int idNotificacion, int idUsuario)
    {
        var notificacion = await _unitOfWork.Notificaciones.ObtenerPorIdAsync(idNotificacion);

        if (notificacion == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Notificación no encontrada." };

        if (notificacion.IdUsuarioDestinatario != idUsuario)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "No tenés permiso sobre esta notificación." };

        notificacion.Leida = true;
        _unitOfWork.Notificaciones.Actualizar(notificacion);
        await _unitOfWork.GuardarCambiosAsync();

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    public async Task<BaseResponse<bool>> MarcarTodasComoLeidasAsync(int idUsuario)
    {
        var notificaciones = await _unitOfWork.Notificaciones.ObtenerTodosAsync();
        var noLeidas = notificaciones.Where(n => n.IdUsuarioDestinatario == idUsuario && !n.Leida).ToList();

        if (!noLeidas.Any())
            return new BaseResponse<bool> { Success = true, Data = true };

        foreach (var item in noLeidas)
        {
            item.Leida = true;
            _unitOfWork.Notificaciones.Actualizar(item);
        }

        await _unitOfWork.GuardarCambiosAsync();
        _logger.LogInformation("Todas las notificaciones marcadas como leídas para el usuario {IdUsuario}", idUsuario);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    public async Task<BaseResponse<bool>> EliminarAsync(int idNotificacion, int idUsuario)
    {
        var notificacion = await _unitOfWork.Notificaciones.ObtenerPorIdAsync(idNotificacion);

        if (notificacion == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Notificación no encontrada." };

        if (notificacion.IdUsuarioDestinatario != idUsuario)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "No tenés autorización para eliminar esta notificación." };

        _unitOfWork.Notificaciones.Eliminar(notificacion);
        await _unitOfWork.GuardarCambiosAsync();

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    private static NotificacionResponseDto MapearADto(Notificacion n) => new()
    {
        IdNotificacion = n.IdNotificacion,
        IdUsuarioDestinatario = n.IdUsuarioDestinatario,
        Titulo = n.Titulo,
        Mensaje = n.Mensaje,
        Tipo = n.Tipo,
        ReferenciaId = n.ReferenciaId,
        Leida = n.Leida,
        FechaCreacion = n.FechaCreacion
    };
}