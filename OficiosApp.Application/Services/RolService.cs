using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Roles;

namespace OficiosApp.Application.Services;

public class RolService : IRolService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RolService> _logger;

    public RolService(IUnitOfWork unitOfWork, ILogger<RolService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<RolResponseDto>>> ObtenerTodosAsync()
    {
        var roles = await _unitOfWork.Roles.ObtenerTodosAsync();

        var lista = roles
            .OrderBy(r => r.IdRol)
            .Select(MapearADto)
            .ToList();

        return new BaseResponse<IEnumerable<RolResponseDto>>
        {
            Success = true,
            Data = lista
        };
    }

    public async Task<BaseResponse<RolResponseDto>> ObtenerPorIdAsync(int idRol)
    {
        var rol = await _unitOfWork.Roles.ObtenerPorIdAsync(idRol);

        if (rol == null)
            return new BaseResponse<RolResponseDto> { Success = false, ErrorMessage = "Rol no encontrado." };

        return new BaseResponse<RolResponseDto> { Success = true, Data = MapearADto(rol) };
    }

    private static RolResponseDto MapearADto(Rol r) => new()
    {
        IdRol = r.IdRol,
        Nombre = r.Nombre,
        Descripcion = r.Descripcion
    };
}