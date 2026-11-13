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

    public async Task<(bool Exito, string Mensaje)> CrearCitaAsync(CrearCitaDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/citas", dto);
            if (response.IsSuccessStatusCode)
            {
                return (true, "Solicitud de cita enviada con éxito.");
            }

            var resultado = await response.Content.ReadFromJsonAsync<BaseResponse<CitaResponseDto>>();
            return (false, resultado?.ErrorMessage ?? "No se pudo registrar la cita.");
        }
        catch (Exception ex)
        {
            return (false, "Error de conexión: " + ex.Message);
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

    public async Task<List<HistorialEstadoCitaResponseDto>> ObtenerHistorialCitaAsync(int idCita)
    {
        try
        {
            var respuesta = await _httpClient.GetFromJsonAsync<BaseResponse<List<HistorialEstadoCitaResponseDto>>>($"api/citas/{idCita}/historial");
            if (respuesta != null && respuesta.Success && respuesta.Data != null)
            {
                return respuesta.Data;
            }
            return new List<HistorialEstadoCitaResponseDto>();
        }
        catch
        {
            return new List<HistorialEstadoCitaResponseDto>();
        }
    }
}
