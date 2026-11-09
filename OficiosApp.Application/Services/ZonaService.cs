using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Zonas;

namespace OficiosApp.Application.Services;

public class ZonaService : IZonaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ZonaService> _logger;

    public ZonaService(IUnitOfWork unitOfWork, ILogger<ZonaService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<ZonaResponseDto>>> ObtenerTodasAsync(bool soloActivas = true)
    {
        var zonas = await _unitOfWork.Zonas.ObtenerTodosAsync();

        var query = zonas.AsQueryable();
        if (soloActivas)
            query = query.Where(z => z.Activa);

        var lista = query.Select(MapearADto).ToList();

        return new BaseResponse<IEnumerable<ZonaResponseDto>>
        {
            Success = true,
            Data = lista
        };
    }

    public async Task<BaseResponse<ZonaResponseDto>> ObtenerPorIdAsync(int idZona)
    {
        var zona = await _unitOfWork.Zonas.ObtenerPorIdAsync(idZona);

        if (zona == null)
            return new BaseResponse<ZonaResponseDto> { Success = false, ErrorMessage = "Zona no encontrada." };

        return new BaseResponse<ZonaResponseDto> { Success = true, Data = MapearADto(zona) };
    }

    public async Task<BaseResponse<ZonaResponseDto>> CrearAsync(CrearZonaDto dto)
    {
        var nuevaZona = new Zona
        {
            Nombre = dto.Nombre.Trim(),
            CodigoPostal = dto.CodigoPostal?.Trim(),
            Activa = true
        };

        await _unitOfWork.Zonas.AgregarAsync(nuevaZona);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Nueva zona creada con ID {IdZona}: {Nombre}", nuevaZona.IdZona, nuevaZona.Nombre);

        return new BaseResponse<ZonaResponseDto> { Success = true, Data = MapearADto(nuevaZona) };
    }

    public async Task<BaseResponse<ZonaResponseDto>> ActualizarAsync(int idZona, ActualizarZonaDto dto)
    {
        var zona = await _unitOfWork.Zonas.ObtenerPorIdAsync(idZona);

        if (zona == null)
            return new BaseResponse<ZonaResponseDto> { Success = false, ErrorMessage = "Zona no encontrada." };

        zona.Nombre = dto.Nombre.Trim();
        zona.CodigoPostal = dto.CodigoPostal?.Trim();
        zona.Activa = dto.Activa;

        _unitOfWork.Zonas.Actualizar(zona);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Zona con ID {IdZona} actualizada", idZona);

        return new BaseResponse<ZonaResponseDto> { Success = true, Data = MapearADto(zona) };
    }

    public async Task<BaseResponse<bool>> EliminarAsync(int idZona)
    {
        var zona = await _unitOfWork.Zonas.ObtenerPorIdAsync(idZona);

        if (zona == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Zona no encontrada." };

        // Borrado lógico para proteger la integridad con ProfesionalZona
        zona.Activa = false;
        _unitOfWork.Zonas.Actualizar(zona);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Zona con ID {IdZona} desactivada", idZona);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    private static ZonaResponseDto MapearADto(Zona z) => new()
    {
        IdZona = z.IdZona,
        Nombre = z.Nombre,
        CodigoPostal = z.CodigoPostal,
        Activa = z.Activa
    };
}