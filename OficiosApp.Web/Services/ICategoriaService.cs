using OficiosApp.Shared.Dtos.Categoria;

namespace OficiosApp.Web.Services;

public interface ICategoriaService
{
    Task<List<CategoriaResponseDto>> ObtenerTodasAsync(bool soloActivas = false);
    Task<bool> CrearAsync(CrearCategoriaDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarCategoriaDto dto);
    Task<bool> EliminarAsync(int id);
}