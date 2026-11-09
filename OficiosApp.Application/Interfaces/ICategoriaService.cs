using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Categoria;

namespace OficiosApp.Application.Interfaces;

public interface ICategoriaService
{
    Task<BaseResponse<IEnumerable<CategoriaResponseDto>>> ObtenerTodasAsync(bool soloActivas = true);
    Task<BaseResponse<CategoriaResponseDto>> ObtenerPorIdAsync(int idCategoria);
    Task<BaseResponse<CategoriaResponseDto>> CrearAsync(CrearCategoriaDto dto);
    Task<BaseResponse<CategoriaResponseDto>> ActualizarAsync(int idCategoria, ActualizarCategoriaDto dto);
    Task<BaseResponse<bool>> EliminarAsync(int idCategoria); // Borrado lógico
}