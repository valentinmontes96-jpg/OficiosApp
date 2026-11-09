using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Portfolio;

namespace OficiosApp.Application.Services;

public class FotoPortfolioService : IFotoPortfolioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FotoPortfolioService> _logger;

    public FotoPortfolioService(IUnitOfWork unitOfWork, ILogger<FotoPortfolioService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<FotoPortfolioResponseDto>>> ObtenerPorProfesionalAsync(int idUsuarioProfesional)
    {
        var fotos = await _unitOfWork.FotosPortfolio.ObtenerTodosAsync();

        var lista = fotos
            .Where(f => f.IdUsuarioProfesional == idUsuarioProfesional)
            .OrderByDescending(f => f.FechaCarga)
            .Select(MapearADto)
            .ToList();

        return new BaseResponse<IEnumerable<FotoPortfolioResponseDto>>
        {
            Success = true,
            Data = lista
        };
    }

    public async Task<BaseResponse<FotoPortfolioResponseDto>> ObtenerPorIdAsync(int idFoto)
    {
        var foto = await _unitOfWork.FotosPortfolio.ObtenerPorIdAsync(idFoto);

        if (foto == null)
            return new BaseResponse<FotoPortfolioResponseDto> { Success = false, ErrorMessage = "Foto no encontrada." };

        return new BaseResponse<FotoPortfolioResponseDto> { Success = true, Data = MapearADto(foto) };
    }

    public async Task<BaseResponse<FotoPortfolioResponseDto>> AgregarAsync(int idUsuarioProfesional, CrearFotoPortfolioDto dto)
    {
        var profesional = await _unitOfWork.Profesionales.ObtenerPorIdAsync(idUsuarioProfesional);
        if (profesional == null)
            return new BaseResponse<FotoPortfolioResponseDto> { Success = false, ErrorMessage = "El profesional no existe." };

        var nuevaFoto = new FotoPortfolio
        {
            IdUsuarioProfesional = idUsuarioProfesional,
            ImagenUrl = dto.ImagenUrl.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            FechaCarga = DateTime.UtcNow
        };

        await _unitOfWork.FotosPortfolio.AgregarAsync(nuevaFoto);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Foto con ID {IdFoto} agregada al portfolio del profesional {IdProfesional}", nuevaFoto.IdFoto, idUsuarioProfesional);

        return new BaseResponse<FotoPortfolioResponseDto> { Success = true, Data = MapearADto(nuevaFoto) };
    }

    public async Task<BaseResponse<FotoPortfolioResponseDto>> ActualizarDescripcionAsync(int idFoto, int idUsuarioProfesional, ActualizarFotoPortfolioDto dto)
    {
        var foto = await _unitOfWork.FotosPortfolio.ObtenerPorIdAsync(idFoto);

        if (foto == null)
            return new BaseResponse<FotoPortfolioResponseDto> { Success = false, ErrorMessage = "Foto no encontrada." };

        if (foto.IdUsuarioProfesional != idUsuarioProfesional)
            return new BaseResponse<FotoPortfolioResponseDto> { Success = false, ErrorMessage = "No tenés permiso para editar esta foto." };

        foto.Descripcion = dto.Descripcion?.Trim();

        _unitOfWork.FotosPortfolio.Actualizar(foto);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Descripción de la foto {IdFoto} actualizada por el profesional {IdProfesional}", idFoto, idUsuarioProfesional);

        return new BaseResponse<FotoPortfolioResponseDto> { Success = true, Data = MapearADto(foto) };
    }

    public async Task<BaseResponse<bool>> EliminarAsync(int idFoto, int idUsuarioProfesional)
    {
        var foto = await _unitOfWork.FotosPortfolio.ObtenerPorIdAsync(idFoto);

        if (foto == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Foto no encontrada." };

        if (foto.IdUsuarioProfesional != idUsuarioProfesional)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "No tenés autorización para eliminar esta foto." };

        _unitOfWork.FotosPortfolio.Eliminar(foto);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Foto {IdFoto} eliminada del portfolio por el profesional {IdProfesional}", idFoto, idUsuarioProfesional);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    private static FotoPortfolioResponseDto MapearADto(FotoPortfolio f) => new()
    {
        IdFoto = f.IdFoto,
        IdUsuarioProfesional = f.IdUsuarioProfesional,
        ImagenUrl = f.ImagenUrl,
        Descripcion = f.Descripcion,
        FechaCarga = f.FechaCarga
    };
}