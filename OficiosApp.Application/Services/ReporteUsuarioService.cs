using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Reportes;

namespace OficiosApp.Application.Services;

public class ReporteUsuarioService : IReporteUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReporteUsuarioService> _logger;

    public ReporteUsuarioService(IUnitOfWork unitOfWork, ILogger<ReporteUsuarioService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<ReporteUsuarioResponseDto>>> ObtenerTodosAsync(string? estado = null)
    {
        var reportes = await _unitOfWork.ReportesUsuarios.ObtenerTodosAsync();

        var query = reportes.AsQueryable();
        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(r => r.Estado.Equals(estado.Trim(), StringComparison.OrdinalIgnoreCase));

        var lista = query
            .OrderByDescending(r => r.FechaReporte)
            .Select(MapearADto)
            .ToList();

        return new BaseResponse<IEnumerable<ReporteUsuarioResponseDto>>
        {
            Success = true,
            Data = lista
        };
    }

    public async Task<BaseResponse<ReporteUsuarioResponseDto>> ObtenerPorIdAsync(int idReporte)
    {
        var reporte = await _unitOfWork.ReportesUsuarios.ObtenerPorIdAsync(idReporte);

        if (reporte == null)
            return new BaseResponse<ReporteUsuarioResponseDto> { Success = false, ErrorMessage = "Reporte no encontrado." };

        return new BaseResponse<ReporteUsuarioResponseDto> { Success = true, Data = MapearADto(reporte) };
    }

    public async Task<BaseResponse<ReporteUsuarioResponseDto>> CrearAsync(int idUsuarioDenunciante, CrearReporteDto dto)
    {
        if (idUsuarioDenunciante == dto.IdUsuarioDenunciado)
        {
            return new BaseResponse<ReporteUsuarioResponseDto>
            {
                Success = false,
                ErrorMessage = "No podés denunciar a tu propia cuenta."
            };
        }

        var denunciado = await _unitOfWork.Usuarios.ObtenerPorIdAsync(dto.IdUsuarioDenunciado);
        if (denunciado == null)
        {
            return new BaseResponse<ReporteUsuarioResponseDto>
            {
                Success = false,
                ErrorMessage = "El usuario denunciado no existe."
            };
        }

        var nuevoReporte = new ReporteUsuario
        {
            IdUsuarioDenunciante = idUsuarioDenunciante,
            IdUsuarioDenunciado = dto.IdUsuarioDenunciado,
            Motivo = dto.Motivo.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            Estado = "Pendiente",
            FechaReporte = DateTime.UtcNow
        };

        await _unitOfWork.ReportesUsuarios.AgregarAsync(nuevoReporte);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogWarning("Nuevo reporte {IdReporte}: Usuario {IdDenunciante} denunció al usuario {IdDenunciado}. Motivo: {Motivo}",
            nuevoReporte.IdReporte, idUsuarioDenunciante, dto.IdUsuarioDenunciado, nuevoReporte.Motivo);

        return new BaseResponse<ReporteUsuarioResponseDto> { Success = true, Data = MapearADto(nuevoReporte) };
    }

    public async Task<BaseResponse<bool>> ResolverAsync(int idReporte, int idAdministrador, ResolverReporteDto dto)
    {
        var reporte = await _unitOfWork.ReportesUsuarios.ObtenerPorIdAsync(idReporte);

        if (reporte == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Reporte no encontrado." };

        reporte.Estado = dto.Estado.Trim();
        reporte.IdAdministradorResolutor = idAdministrador;

        _unitOfWork.ReportesUsuarios.Actualizar(reporte);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Reporte {IdReporte} resuelto por administrador {IdAdmin}. Nuevo estado: {Estado}",
            idReporte, idAdministrador, reporte.Estado);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    private static ReporteUsuarioResponseDto MapearADto(ReporteUsuario r) => new()
    {
        IdReporte = r.IdReporte,
        IdUsuarioDenunciante = r.IdUsuarioDenunciante,
        NombreDenunciante = $"{r.Denunciante?.Nombre} {r.Denunciante?.Apellido}".Trim(),
        IdUsuarioDenunciado = r.IdUsuarioDenunciado,
        NombreDenunciado = $"{r.Denunciado?.Nombre} {r.Denunciado?.Apellido}".Trim(),
        Motivo = r.Motivo,
        Descripcion = r.Descripcion,
        FechaReporte = r.FechaReporte,
        Estado = r.Estado,
        IdAdministradorResolutor = r.IdAdministradorResolutor
    };
}