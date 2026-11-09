using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Citas;
using OficiosApp.Shared.Dtos.Citas.Response;

namespace OficiosApp.Application.Services;

public class CitaService : ICitaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CitaService> _logger;

    public CitaService(IUnitOfWork unitOfWork, ILogger<CitaService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<CitaResponseDto>>> ObtenerTodasAsync()
    {
        var citas = await _unitOfWork.Citas.ObtenerTodosAsync();
        return new BaseResponse<IEnumerable<CitaResponseDto>>
        {
            Success = true,
            Data = citas.Select(MapearADto).ToList()
        };
    }

    public async Task<BaseResponse<CitaResponseDto>> ObtenerPorIdAsync(int idCita)
    {
        var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(idCita);
        if (cita == null)
            return new BaseResponse<CitaResponseDto> { Success = false, ErrorMessage = "Cita no encontrada." };

        return new BaseResponse<CitaResponseDto> { Success = true, Data = MapearADto(cita) };
    }

    public async Task<BaseResponse<IEnumerable<CitaResponseDto>>> ObtenerPorClienteAsync(int idUsuarioCliente)
    {
        var citas = await _unitOfWork.Citas.ObtenerTodosAsync();
        var lista = citas.Where(c => c.IdUsuarioCliente == idUsuarioCliente).Select(MapearADto).ToList();

        return new BaseResponse<IEnumerable<CitaResponseDto>> { Success = true, Data = lista };
    }

    public async Task<BaseResponse<IEnumerable<CitaResponseDto>>> ObtenerPorProfesionalAsync(int idUsuarioProfesional)
    {
        var citas = await _unitOfWork.Citas.ObtenerTodosAsync();
        var lista = citas.Where(c => c.IdUsuarioProfesional == idUsuarioProfesional).Select(MapearADto).ToList();

        return new BaseResponse<IEnumerable<CitaResponseDto>> { Success = true, Data = lista };
    }

    public async Task<BaseResponse<CitaResponseDto>> CrearAsync(int idUsuarioCliente, CrearCitaDto dto, int idEstadoInicial = 1)
    {
        var nuevaCita = new Cita
        {
            IdUsuarioCliente = idUsuarioCliente,
            IdUsuarioProfesional = dto.IdUsuarioProfesional,
            IdEstadoCita = idEstadoInicial,
            FechaSolicitud = DateTime.UtcNow,
            FechaServicioPactada = dto.FechaServicioPactada,
            DescripcionProblema = dto.DescripcionProblema,
            DireccionServicio = dto.DireccionServicio
        };

        await _unitOfWork.Citas.AgregarAsync(nuevaCita);
        await _unitOfWork.GuardarCambiosAsync();

        var historialInicial = new HistorialEstadoCita
        {
            IdCita = nuevaCita.IdCita,
            IdEstadoCita = idEstadoInicial,
            IdUsuarioAccion = idUsuarioCliente,
            FechaCambio = DateTime.UtcNow,
            Observaciones = "Solicitud creada por el cliente."
        };

        await _unitOfWork.HistorialEstadosCita.AgregarAsync(historialInicial);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Cita {IdCita} creada. Historial inicial registrado con usuario {IdUsuario}", nuevaCita.IdCita, idUsuarioCliente);

        return new BaseResponse<CitaResponseDto> { Success = true, Data = MapearADto(nuevaCita) };
    }

    public async Task<BaseResponse<CitaResponseDto>> ActualizarAsync(int idCita, ActualizarCitaDto dto)
    {
        var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(idCita);
        if (cita == null)
        {
            _logger.LogWarning("Intento de actualización sobre cita inexistente. ID: {IdCita}", idCita);
            return new BaseResponse<CitaResponseDto> { Success = false, ErrorMessage = "Cita no encontrada." };
        }

        cita.FechaServicioPactada = dto.FechaServicioPactada;
        cita.DescripcionProblema = dto.DescripcionProblema;
        cita.DireccionServicio = dto.DireccionServicio;

        if (dto.FechaRespuesta.HasValue)
            cita.FechaRespuesta = dto.FechaRespuesta.Value;

        _unitOfWork.Citas.Actualizar(cita);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Cita con ID {IdCita} actualizada correctamente", idCita);

        return new BaseResponse<CitaResponseDto> { Success = true, Data = MapearADto(cita) };
    }

    public async Task<BaseResponse<bool>> CambiarEstadoAsync(int idCita, int idUsuarioAccion, int nuevoIdEstado, string? observaciones = null)
    {
        var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(idCita);
        if (cita == null)
        {
            _logger.LogWarning("Intento de cambio de estado sobre cita inexistente. ID: {IdCita}", idCita);
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Cita no encontrada." };
        }

        if (cita.IdEstadoCita == nuevoIdEstado)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "La cita ya se encuentra en ese estado." };

        var estadoAnterior = cita.IdEstadoCita;
        cita.IdEstadoCita = nuevoIdEstado;
        cita.FechaRespuesta = DateTime.UtcNow;

        _unitOfWork.Citas.Actualizar(cita);

        var entradaHistorial = new HistorialEstadoCita
        {
            IdCita = idCita,
            IdEstadoCita = nuevoIdEstado,
            IdUsuarioAccion = idUsuarioAccion,
            FechaCambio = DateTime.UtcNow,
            Observaciones = observaciones?.Trim()
        };

        await _unitOfWork.HistorialEstadosCita.AgregarAsync(entradaHistorial);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Cita {IdCita}: Estado cambiado de {EstadoAnterior} a {NuevoEstado} por usuario {IdUsuario}",
            idCita, estadoAnterior, nuevoIdEstado, idUsuarioAccion);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    public async Task<BaseResponse<IEnumerable<HistorialEstadoCitaResponseDto>>> ObtenerHistorialAsync(int idCita)
    {
        var historiales = await _unitOfWork.HistorialEstadosCita.ObtenerTodosAsync();

        var lista = historiales
            .Where(h => h.IdCita == idCita)
            .OrderByDescending(h => h.FechaCambio)
            .Select(h => new HistorialEstadoCitaResponseDto
            {
                IdHistorial = h.IdHistorial,
                IdCita = h.IdCita,
                IdEstadoCita = h.IdEstadoCita,
                NombreEstado = h.Estado?.Nombre ?? $"Estado #{h.IdEstadoCita}",
                IdUsuarioAccion = h.IdUsuarioAccion,
                NombreUsuarioAccion = $"{h.UsuarioAccion?.Nombre} {h.UsuarioAccion?.Apellido}".Trim(),
                FechaCambio = h.FechaCambio,
                Observaciones = h.Observaciones
            })
            .ToList();

        return new BaseResponse<IEnumerable<HistorialEstadoCitaResponseDto>>
        {
            Success = true,
            Data = lista
        };
    }

    private static CitaResponseDto MapearADto(Cita c) => new()
    {
        IdCita = c.IdCita,
        IdUsuarioCliente = c.IdUsuarioCliente,
        IdUsuarioProfesional = c.IdUsuarioProfesional,
        IdEstadoCita = c.IdEstadoCita,
        FechaSolicitud = c.FechaSolicitud,
        FechaServicioPactada = c.FechaServicioPactada,
        DescripcionProblema = c.DescripcionProblema,
        DireccionServicio = c.DireccionServicio,
        FechaRespuesta = c.FechaRespuesta
    };
}