using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.HorariosAtencion;

namespace OficiosApp.Application.Services;

public class HorarioAtencionService : IHorarioAtencionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HorarioAtencionService> _logger;

    public HorarioAtencionService(IUnitOfWork unitOfWork, ILogger<HorarioAtencionService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<HorarioAtencionResponseDto>>> ObtenerPorProfesionalAsync(int idUsuarioProfesional)
    {
        var horarios = await _unitOfWork.HorariosAtencion.ObtenerTodosAsync();

        var dtos = horarios
            .Where(h => h.IdUsuarioProfesional == idUsuarioProfesional)
            .OrderBy(h => h.DiaSemana)
            .ThenBy(h => h.HoraApertura)
            .Select(MapearADto)
            .ToList();

        return new BaseResponse<IEnumerable<HorarioAtencionResponseDto>>
        {
            Success = true,
            Data = dtos
        };
    }

    public async Task<BaseResponse<HorarioAtencionResponseDto>> GuardarDiaAsync(int idUsuarioProfesional, CrearOActualizarHorarioDto dto)
    {
        if (dto.HoraApertura >= dto.HoraCierre)
        {
            return new BaseResponse<HorarioAtencionResponseDto>
            {
                Success = false,
                ErrorMessage = "La hora de apertura debe ser menor a la hora de cierre."
            };
        }

        var todos = await _unitOfWork.HorariosAtencion.ObtenerTodosAsync();
        var existente = todos.FirstOrDefault(h => h.IdUsuarioProfesional == idUsuarioProfesional && h.DiaSemana == dto.DiaSemana);

        if (existente != null)
        {
            existente.HoraApertura = dto.HoraApertura;
            existente.HoraCierre = dto.HoraCierre;
            _unitOfWork.HorariosAtencion.Actualizar(existente);
            await _unitOfWork.GuardarCambiosAsync();

            _logger.LogInformation("Horario actualizado para profesional {IdUsuario} en día {Dia}", idUsuarioProfesional, dto.DiaSemana);
            return new BaseResponse<HorarioAtencionResponseDto> { Success = true, Data = MapearADto(existente) };
        }

        var nuevo = new HorarioAtencion
        {
            IdUsuarioProfesional = idUsuarioProfesional,
            DiaSemana = dto.DiaSemana,
            HoraApertura = dto.HoraApertura,
            HoraCierre = dto.HoraCierre
        };

        await _unitOfWork.HorariosAtencion.AgregarAsync(nuevo);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Nuevo horario creado con ID {IdHorario} para profesional {IdUsuario}", nuevo.IdHorario, idUsuarioProfesional);

        return new BaseResponse<HorarioAtencionResponseDto> { Success = true, Data = MapearADto(nuevo) };
    }

    public async Task<BaseResponse<bool>> SincronizarSemanaAsync(int idUsuarioProfesional, List<CrearOActualizarHorarioDto> listaHorarios)
    {
        foreach (var item in listaHorarios)
        {
            if (item.HoraApertura >= item.HoraCierre)
            {
                return new BaseResponse<bool>
                {
                    Success = false,
                    ErrorMessage = $"Inconsistencia en día {item.DiaSemana}: La hora de apertura debe ser anterior a la de cierre."
                };
            }
        }

        var todos = await _unitOfWork.HorariosAtencion.ObtenerTodosAsync();
        var horariosActuales = todos.Where(h => h.IdUsuarioProfesional == idUsuarioProfesional).ToList();

        // Limpiar los horarios previos
        foreach (var viejo in horariosActuales)
        {
            _unitOfWork.HorariosAtencion.Eliminar(viejo);
        }

        // Insertar la nueva grilla
        foreach (var item in listaHorarios)
        {
            var nuevo = new HorarioAtencion
            {
                IdUsuarioProfesional = idUsuarioProfesional,
                DiaSemana = item.DiaSemana,
                HoraApertura = item.HoraApertura,
                HoraCierre = item.HoraCierre
            };
            await _unitOfWork.HorariosAtencion.AgregarAsync(nuevo);
        }

        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Grilla semanal completa actualizada para el profesional {IdUsuario}", idUsuarioProfesional);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    public async Task<BaseResponse<bool>> EliminarAsync(int idHorario, int idUsuarioProfesional)
    {
        var horario = await _unitOfWork.HorariosAtencion.ObtenerPorIdAsync(idHorario);

        if (horario == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Horario no encontrado." };

        if (horario.IdUsuarioProfesional != idUsuarioProfesional)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "No tenés permiso para eliminar este horario." };

        _unitOfWork.HorariosAtencion.Eliminar(horario);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Horario {IdHorario} eliminado por el profesional {IdUsuario}", idHorario, idUsuarioProfesional);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    private static HorarioAtencionResponseDto MapearADto(HorarioAtencion h) => new()
    {
        IdHorario = h.IdHorario,
        IdUsuarioProfesional = h.IdUsuarioProfesional,
        DiaSemana = h.DiaSemana,
        HoraApertura = h.HoraApertura,
        HoraCierre = h.HoraCierre
    };
}