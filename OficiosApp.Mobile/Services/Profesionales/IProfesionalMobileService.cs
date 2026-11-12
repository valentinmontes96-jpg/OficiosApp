using OficiosApp.Shared.Dtos.Profesionales;

namespace OficiosApp.Mobile.Services;

public interface IProfesionalMobileService
{
    Task<ProfesionalResponseDto?> ObtenerPerfilAsync(int idUsuario);
    Task<ProfesionalResponseDto?> ActualizarPerfilAsync(ActualizarProfesionalDto dto);
}

