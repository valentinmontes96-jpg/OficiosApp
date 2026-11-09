using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Favoritos;

namespace OficiosApp.Application.Services;

public class FavoritoService : IFavoritoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FavoritoService> _logger;

    public FavoritoService(IUnitOfWork unitOfWork, ILogger<FavoritoService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<FavoritoResponseDto>>> ObtenerMisFavoritosAsync(int idUsuarioCliente)
    {
        var favoritos = await _unitOfWork.Favoritos.ObtenerTodosAsync();

        var lista = favoritos
            .Where(f => f.IdUsuarioCliente == idUsuarioCliente)
            .Select(f => new FavoritoResponseDto
            {
                IdUsuarioProfesional = f.IdUsuarioProfesional,
                NombreProfesional = f.Profesional?.Usuario?.Nombre ?? string.Empty,
                ApellidoProfesional = f.Profesional?.Usuario?.Apellido ?? string.Empty,
                TarifaBase = f.Profesional?.TarifaBase,
                DisponibleUrgencia = f.Profesional?.DisponibleUrgencia ?? false,
                FechaAgregado = f.FechaAgregado
            })
            .ToList();

        return new BaseResponse<IEnumerable<FavoritoResponseDto>>
        {
            Success = true,
            Data = lista
        };
    }

    public async Task<BaseResponse<bool>> EsFavoritoAsync(int idUsuarioCliente, int idUsuarioProfesional)
    {
        var favoritos = await _unitOfWork.Favoritos.ObtenerTodosAsync();
        var existe = favoritos.Any(f => f.IdUsuarioCliente == idUsuarioCliente && f.IdUsuarioProfesional == idUsuarioProfesional);

        return new BaseResponse<bool> { Success = true, Data = existe };
    }

    public async Task<BaseResponse<bool>> AgregarAsync(int idUsuarioCliente, int idUsuarioProfesional)
    {
        if (idUsuarioCliente == idUsuarioProfesional)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "No podés agregarte a vos mismo a favoritos." };

        var profesional = await _unitOfWork.Profesionales.ObtenerPorIdAsync(idUsuarioProfesional);
        if (profesional == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "El profesional no existe." };

        var favoritos = await _unitOfWork.Favoritos.ObtenerTodosAsync();
        var yaExiste = favoritos.Any(f => f.IdUsuarioCliente == idUsuarioCliente && f.IdUsuarioProfesional == idUsuarioProfesional);

        if (yaExiste)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "El profesional ya está en tu lista de favoritos." };

        var nuevoFavorito = new Favorito
        {
            IdUsuarioCliente = idUsuarioCliente,
            IdUsuarioProfesional = idUsuarioProfesional,
            FechaAgregado = DateTime.UtcNow
        };

        await _unitOfWork.Favoritos.AgregarAsync(nuevoFavorito);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Cliente {IdCliente} guardó en favoritos al profesional {IdProfesional}", idUsuarioCliente, idUsuarioProfesional);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    public async Task<BaseResponse<bool>> EliminarAsync(int idUsuarioCliente, int idUsuarioProfesional)
    {
        var favoritos = await _unitOfWork.Favoritos.ObtenerTodosAsync();
        var favorito = favoritos.FirstOrDefault(f => f.IdUsuarioCliente == idUsuarioCliente && f.IdUsuarioProfesional == idUsuarioProfesional);

        if (favorito == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "El registro de favorito no existe." };

        _unitOfWork.Favoritos.Eliminar(favorito);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Cliente {IdCliente} eliminó de favoritos al profesional {IdProfesional}", idUsuarioCliente, idUsuarioProfesional);

        return new BaseResponse<bool> { Success = true, Data = true };
    }
}