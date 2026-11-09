using Microsoft.Extensions.Logging;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Categoria;

namespace OficiosApp.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoriaService> _logger;

    public CategoriaService(IUnitOfWork unitOfWork, ILogger<CategoriaService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<CategoriaResponseDto>>> ObtenerTodasAsync(bool soloActivas = true)
    {
        var categorias = await _unitOfWork.Categorias.ObtenerTodosAsync();

        var query = categorias.AsQueryable();
        if (soloActivas)
            query = query.Where(c => c.Activa);

        var lista = query
            .OrderBy(c => c.Nombre)
            .Select(MapearADto)
            .ToList();

        return new BaseResponse<IEnumerable<CategoriaResponseDto>>
        {
            Success = true,
            Data = lista
        };
    }

    public async Task<BaseResponse<CategoriaResponseDto>> ObtenerPorIdAsync(int idCategoria)
    {
        var categoria = await _unitOfWork.Categorias.ObtenerPorIdAsync(idCategoria);

        if (categoria == null)
            return new BaseResponse<CategoriaResponseDto> { Success = false, ErrorMessage = "Categoría no encontrada." };

        return new BaseResponse<CategoriaResponseDto> { Success = true, Data = MapearADto(categoria) };
    }

    public async Task<BaseResponse<CategoriaResponseDto>> CrearAsync(CrearCategoriaDto dto)
    {
        var todas = await _unitOfWork.Categorias.ObtenerTodosAsync();
        if (todas.Any(c => c.Nombre.Trim().Equals(dto.Nombre.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            return new BaseResponse<CategoriaResponseDto>
            {
                Success = false,
                ErrorMessage = "Ya existe una categoría con ese nombre."
            };
        }

        var nuevaCategoria = new Categoria
        {
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            IconoUrl = dto.IconoUrl?.Trim(),
            Activa = true
        };

        await _unitOfWork.Categorias.AgregarAsync(nuevaCategoria);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Categoría creada con ID {IdCategoria}: {Nombre}", nuevaCategoria.IdCategoria, nuevaCategoria.Nombre);

        return new BaseResponse<CategoriaResponseDto> { Success = true, Data = MapearADto(nuevaCategoria) };
    }

    public async Task<BaseResponse<CategoriaResponseDto>> ActualizarAsync(int idCategoria, ActualizarCategoriaDto dto)
    {
        var categoria = await _unitOfWork.Categorias.ObtenerPorIdAsync(idCategoria);
        if (categoria == null)
            return new BaseResponse<CategoriaResponseDto> { Success = false, ErrorMessage = "Categoría no encontrada." };

        var todas = await _unitOfWork.Categorias.ObtenerTodosAsync();
        if (todas.Any(c => c.IdCategoria != idCategoria && c.Nombre.Trim().Equals(dto.Nombre.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            return new BaseResponse<CategoriaResponseDto>
            {
                Success = false,
                ErrorMessage = "Ya existe otra categoría con ese nombre."
            };
        }

        categoria.Nombre = dto.Nombre.Trim();
        categoria.Descripcion = dto.Descripcion?.Trim();
        categoria.IconoUrl = dto.IconoUrl?.Trim();
        categoria.Activa = dto.Activa;

        _unitOfWork.Categorias.Actualizar(categoria);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Categoría con ID {IdCategoria} actualizada", idCategoria);

        return new BaseResponse<CategoriaResponseDto> { Success = true, Data = MapearADto(categoria) };
    }

    public async Task<BaseResponse<bool>> EliminarAsync(int idCategoria)
    {
        var categoria = await _unitOfWork.Categorias.ObtenerPorIdAsync(idCategoria);
        if (categoria == null)
            return new BaseResponse<bool> { Success = false, ErrorMessage = "Categoría no encontrada." };

        // Borrado lógico para proteger la relación con ProfesionalCategoria
        categoria.Activa = false;
        _unitOfWork.Categorias.Actualizar(categoria);
        await _unitOfWork.GuardarCambiosAsync();

        _logger.LogInformation("Categoría con ID {IdCategoria} desactivada", idCategoria);

        return new BaseResponse<bool> { Success = true, Data = true };
    }

    private static CategoriaResponseDto MapearADto(Categoria c) => new()
    {
        IdCategoria = c.IdCategoria,
        Nombre = c.Nombre,
        Descripcion = c.Descripcion,
        IconoUrl = c.IconoUrl,
        Activa = c.Activa
    };
}