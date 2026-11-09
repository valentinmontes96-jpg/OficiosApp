using OficiosApp.Shared.Dtos;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Portfolio;

namespace OficiosApp.Application.Interfaces;

public interface IFotoPortfolioService
{
    Task<BaseResponse<IEnumerable<FotoPortfolioResponseDto>>> ObtenerPorProfesionalAsync(int idUsuarioProfesional);
    Task<BaseResponse<FotoPortfolioResponseDto>> ObtenerPorIdAsync(int idFoto);
    Task<BaseResponse<FotoPortfolioResponseDto>> AgregarAsync(int idUsuarioProfesional, CrearFotoPortfolioDto dto);
    Task<BaseResponse<FotoPortfolioResponseDto>> ActualizarDescripcionAsync(int idFoto, int idUsuarioProfesional, ActualizarFotoPortfolioDto dto);
    Task<BaseResponse<bool>> EliminarAsync(int idFoto, int idUsuarioProfesional);
}