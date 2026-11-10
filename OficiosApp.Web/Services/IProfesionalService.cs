using OficiosApp.Shared.Dtos.Profesionales;

namespace OficiosApp.Web.Services;

public interface IProfesionalService
{
    Task<List<ProfesionalResponseDto>> ObtenerTodosAsync();
    Task<ProfesionalResponseDto?> ObtenerPorIdAsync(int id);
}