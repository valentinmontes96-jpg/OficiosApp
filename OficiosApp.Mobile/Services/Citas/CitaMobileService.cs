using System.Net.Http.Json;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Citas;
using OficiosApp.Shared.Dtos.Citas.Response;

namespace OficiosApp.Mobile.Services;

public class CitaMobileService : ICitaMobileService
{
    private readonly HttpClient _httpClient;

    public CitaMobileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CitaResponseDto>> ObtenerMisCitasProfesionalAsync()
    {
        try
        {
            var respuesta = await _httpClient.GetFromJsonAsync<BaseResponse<List<CitaResponseDto>>>("api/citas/mis-citas-profesional");
            if (respuesta != null && respuesta.Success && respuesta.Data != null)
            {
                return respuesta.Data
                    .OrderByDescending(c => c.FechaServicioPactada)
                    .ToList();
            }

            return new List<CitaResponseDto>();
        }
        catch
        {
            return new List<CitaResponseDto>();
        }
    }

    public async Task<List<CitaResponseDto>> ObtenerMisCitasClienteAsync()
    {
        try
        {
            var respuesta = await _httpClient.GetFromJsonAsync<BaseResponse<List<CitaResponseDto>>>("api/citas/mis-citas-cliente");
            if (respuesta != null && respuesta.Success && respuesta.Data != null)
            {
                return respuesta.Data
                    .OrderByDescending(c => c.FechaServicioPactada)
                    .ToList();
            }

            return new List<CitaResponseDto>();
        }
        catch
        {
            return new List<CitaResponseDto>();
        }
    }

    public async Task<bool> CambiarEstadoCitaAsync(int idCita, int nuevoEstadoId, string? observaciones = null)
    {
        try
        {
            var dto = new CambiarEstadoCitaDto
            {
                NuevoEstadoId = nuevoEstadoId,
                Observaciones = observaciones
            };

            var response = await _httpClient.PatchAsJsonAsync($"api/citas/{idCita}/estado", dto);
            if (!response.IsSuccessStatusCode)
                return false;

            var resultado = await response.Content.ReadFromJsonAsync<BaseResponse<bool>>();
            return resultado != null && resultado.Success;
        }
        catch
        {
            return false;
        }
    }
}

