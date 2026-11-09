using OficiosApp.Shared.Dtos.Citas.Request;
using OficiosApp.Shared.Dtos.Citas.Response;
using OficiosApp.Shared.Dtos.Citas;
using OficiosApp.Shared.Wrappers;

namespace OficiosApp.Application.Interfaces;

public interface ICitaService
{
    Task<ApiResponse<CitaResponseDto>> SolicitarCitaAsync(CitaRequestDto request, int idUsuarioCliente);
    Task<ApiResponse<bool>> AceptarCitaAsync(int idCita, int idUsuarioProfesional);
}