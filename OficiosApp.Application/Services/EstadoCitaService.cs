using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Citas.Response;

namespace OficiosApp.Application.Services;

public class EstadoCitaService : IEstadoCitaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EstadoCitaService> _logger;

    public EstadoCitaService(IUnitOfWork unitOfWork, ILogger<EstadoCitaService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<EstadoCitaResponseDto>>> ObtenerTodosAsync()
    {
        var estados = await _unitOfWork.EstadosCita.ObtenerTodosAsync();

        var lista = estados
            .OrderBy(e => e.IdEstadoCita)
            .Select(MapearADto)
            .ToList();

        return new BaseResponse<IEnumerable<EstadoCitaResponseDto>>
        {
            Success = true,
            Data = lista
        };
    }

    public async Task<BaseResponse<EstadoCitaResponseDto>> ObtenerPorIdAsync(int idEstadoCita)
    {
        var estado = await _unitOfWork.EstadosCita.ObtenerPorIdAsync(idEstadoCita);

        if (estado == null)
            return new BaseResponse<EstadoCitaResponseDto> { Success = false, ErrorMessage = "Estado de cita no encontrado." };

        return new BaseResponse<EstadoCitaResponseDto> { Success = true, Data = MapearADto(estado) };
    }

    private static EstadoCitaResponseDto MapearADto(EstadoCita e) => new()
    {
        IdEstadoCita = e.IdEstadoCita,
        Nombre = e.Nombre,
        Descripcion = e.Descripcion
    };
}