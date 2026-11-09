using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Favoritos;

namespace OficiosApp.Application.Interfaces;

public interface IFavoritoService
{
    Task<BaseResponse<IEnumerable<FavoritoResponseDto>>> ObtenerMisFavoritosAsync(int idUsuarioCliente);
    Task<BaseResponse<bool>> EsFavoritoAsync(int idUsuarioCliente, int idUsuarioProfesional);
    Task<BaseResponse<bool>> AgregarAsync(int idUsuarioCliente, int idUsuarioProfesional);
    Task<BaseResponse<bool>> EliminarAsync(int idUsuarioCliente, int idUsuarioProfesional);
}