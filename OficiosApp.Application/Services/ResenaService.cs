using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Resenas;

namespace OficiosApp.Application.Services;

public class ResenaService : IResenaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ResenaService> _logger;

    public ResenaService(IUnitOfWork unitOfWork, ILogger<ResenaService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<ResenaResponseDto>>> ObtenerTodasAsync()
    {
        var resenas = await _unitOfWork.Resenas.ObtenerTodosAsync();
        return new BaseResponse<IEnumerable<ResenaResponseDto>>
        {
            Success = true,
            Data = resenas.Select(MapearADto).ToList()
        };
    }

    public async Task<BaseResponse<ResenaResponseDto>> ObtenerPorIdAsync(int idResena)
    {
        var resena = await _unitOfWork.Resenas.ObtenerPorIdAsync(idResena);
        if (resena == null)
            return new BaseResponse<ResenaResponseDto> { Success = false, ErrorMessage = "Reseña no encontrada." };

        return new BaseResponse<ResenaResponseDto> { Success = true, Data = MapearADto(resena) };
    }

    public async Task<BaseResponse<ResenaResponseDto>> ObtenerPorCitaAsync(int idCita)
    {
        var resenas = await _unitOfWork.Resenas.ObtenerTodosAsync();
        var resena = resenas.FirstOrDefault(r => r.IdCita == idCita);

        if (resena == null)
            return new BaseResponse<ResenaResponseDto> { Success = false, ErrorMessage = "No existe reseña para la cita solicitada." };

        return new BaseResponse<ResenaResponseDto> { Success = true, Data = MapearADto(resena) };
    }

    public async Task<BaseResponse<IEnumerable<ResenaResponseDto>>> ObtenerPorProfesionalAsync(int idUsuarioProfesional)
    {
        var resenas = await _unitOfWork.Resenas.ObtenerTodosAsync();
        var lista = resenas
            .Where(r => r.Cita != null && r.Cita.IdUsuarioProfesional == idUsuarioProfesional)
            .Select(MapearADto)
            .ToList();

        return new BaseResponse<IEnumerable<ResenaResponseDto>> { Success = true, Data = lista };
    }

    public async Task<BaseResponse<ResenaResponseDto>> CrearAsync(int idUsuarioCliente, CrearResenaDto dto)
    {
        var cita = await _unitOfWork.Citas.ObtenerPorIdAsync(dto.IdCita);
        if (cita == null)
            return new BaseResponse<ResenaResponseDto> { Success = false, ErrorMessage = "La cita especificada no existe." };

        if (cita.IdUsuarioCliente != idUsuarioCliente)
            return new BaseResponse<ResenaResponseDto> { Success = false, ErrorMessage = "No tenés autorización para calificar una cita ajena." };

        var resenasExistentes = await _unitOfWork.Resenas.ObtenerTodosAsync();
        if (resenasExistentes.Any(r => r.IdCita == dto.IdCita))
            return new BaseResponse<ResenaResponseDto> { Success = false, ErrorMessage = "Esta cita ya cuenta con una reseña registrada." };

        var nuevaResena = new Resena
        {
            IdCita = dto.IdCita,
            Puntuacion = dto.Puntuacion,
            Comentario = dto.Comentario,
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Resenas.AgregarAsync(nuevaResena);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Reseña creada con ID {IdResena} para la cita {IdCita} con puntaje {Puntuacion}",
            nuevaResena.IdResena, dto.IdCita, dto.Puntuacion);

        return new BaseResponse<ResenaResponseDto> { Success = true, Data = MapearADto(nuevaResena) };
    }

    public async Task<BaseResponse<bool>> EliminarAsync(int idResena)
    {
        var resena = await _unitOfWork.Resenas.ObtenerPorIdAsync(idResena);
        if (resena == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Reseña no encontrada." };

        _unitOfWork.Resenas.Eliminar(resena);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Reseña {IdResena} eliminada", idResena);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    private static ResenaResponseDto MapearADto(Resena r) => new()
    {
        IdResena = r.IdResena,
        IdCita = r.IdCita,
        Puntuacion = r.Puntuacion,
        Comentario = r.Comentario,
        FechaCreacion = r.FechaCreacion
    };
}