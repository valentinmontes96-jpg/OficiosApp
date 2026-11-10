// Services/ICategoriaService.cs
using OficiosApp.Shared.Dtos.Categoria; // O el namespace donde tengas el DTO

namespace OficiosApp.Mobile.Services;

public interface ICategoriaService
{
    Task<List<CategoriaResponseDto>> ObtenerCategoriasAsync();
}