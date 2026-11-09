using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Profesionales;

namespace OficiosApp.Application.Services;

public class ProfesionalService : IProfesionalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProfesionalService> _logger;

    public ProfesionalService(IUnitOfWork unitOfWork, ILogger<ProfesionalService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<ProfesionalResponseDto>>> ObtenerTodosAsync()
    {
        // Dependiendo de tu UnitOfWork, podés necesitar un Include(p => p.Usuario) 
        // para que traiga Nombre y Apellido.
        var profesionales = await _unitOfWork.Profesionales.ObtenerTodosAsync();

        var lista = profesionales.Select(MapearADto).ToList();

        return new BaseResponse<IEnumerable<ProfesionalResponseDto>> { Success = true, Data = lista };
    }

    public async Task<BaseResponse<ProfesionalResponseDto>> ObtenerPorIdAsync(int idUsuario)
    {
        var profesional = await _unitOfWork.Profesionales.ObtenerPorIdAsync(idUsuario);

        if (profesional == null)
            return new BaseResponse<ProfesionalResponseDto> { Success = false, ErrorMessage = "Profesional no encontrado." };

        return new BaseResponse<ProfesionalResponseDto> { Success = true, Data = MapearADto(profesional) };
    }

    public async Task<BaseResponse<ProfesionalResponseDto>> ActualizarPerfilAsync(int idUsuario, ActualizarProfesionalDto dto)
    {
        var profesional = await _unitOfWork.Profesionales.ObtenerPorIdAsync(idUsuario);
        bool esNuevo = false;

        if (profesional == null)
        {
            // Si el registro de extensión no existe, lo creamos
            profesional = new Profesional { IdUsuario = idUsuario };
            esNuevo = true;
        }

        bool cambioUrgencia = profesional.DisponibleUrgencia != dto.DisponibleUrgencia;

        profesional.Biografia = dto.Biografia;
        profesional.TarifaBase = dto.TarifaBase;
        profesional.DisponibleUrgencia = dto.DisponibleUrgencia;
        profesional.UltimaActualizacionDisponibilidad = DateTime.UtcNow;

        if (esNuevo)
            await _unitOfWork.Profesionales.AgregarAsync(profesional);
        else
            _unitOfWork.Profesionales.Actualizar(profesional);

        await _unitOfWork.GuardarCambiosAsync();

        if (cambioUrgencia)
            _logger.LogInformation("Profesional {Id} cambió su disponibilidad de urgencia a {Estado}", idUsuario, dto.DisponibleUrgencia);

        return new BaseResponse<ProfesionalResponseDto> { Success = true, Data = MapearADto(profesional) };
    }

    public async Task<BaseResponse<bool>> AsignarCategoriasAsync(int idUsuario, List<int> idsCategorias)
    {
        var profesional = await _unitOfWork.Profesionales.ObtenerPorIdAsync(idUsuario);
        if (profesional == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Profesional no encontrado." };

        // 1. Limpiar las categorías actuales
        profesional.ProfesionalesCategorias.Clear();

        // 2. Asignar las nuevas
        foreach (var idCat in idsCategorias)
        {
            profesional.ProfesionalesCategorias.Add(new ProfesionalCategoria
            {
                IdUsuarioProfesional = idUsuario,
                IdCategoria = idCat
            });
        }

        _unitOfWork.Profesionales.Actualizar(profesional);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Categorías actualizadas para el profesional {IdUsuario}. Total asignadas: {Cantidad}", idUsuario, idsCategorias.Count);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    public async Task<BaseResponse<bool>> AsignarZonasAsync(int idUsuario, List<int> idsZonas)
    {
        var profesional = await _unitOfWork.Profesionales.ObtenerPorIdAsync(idUsuario);
        if (profesional == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Profesional no encontrado." };

        profesional.ProfesionalesZonas.Clear();

        foreach (var idZona in idsZonas)
        {
            profesional.ProfesionalesZonas.Add(new ProfesionalZona
            {
                IdUsuarioProfesional = idUsuario,
                IdZona = idZona
            });
        }

        _unitOfWork.Profesionales.Actualizar(profesional);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Zonas actualizadas para el profesional {IdUsuario}", idUsuario);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    private static ProfesionalResponseDto MapearADto(Profesional p)
    {
        return new ProfesionalResponseDto
        {
            IdUsuario = p.IdUsuario,
            Nombre = p.Usuario?.Nombre ?? string.Empty,
            Apellido = p.Usuario?.Apellido ?? string.Empty,
            Biografia = p.Biografia,
            TarifaBase = p.TarifaBase,
            DisponibleUrgencia = p.DisponibleUrgencia,
            UltimaActualizacionDisponibilidad = p.UltimaActualizacionDisponibilidad,
            IdsCategorias = p.ProfesionalesCategorias.Select(pc => pc.IdCategoria).ToList(),
            IdsZonas = p.ProfesionalesZonas.Select(pz => pz.IdZona).ToList() 
        };
    }
}