using System.Net.Http.Json;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Zonas;

namespace OficiosApp.Web.Services;

public class ZonaService : IZonaService
{
    private readonly HttpClient _http;

    public ZonaService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ZonaResponseDto>> ObtenerTodasAsync(bool soloActivas = false)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<BaseResponse<List<ZonaResponseDto>>>($"api/Zonas?soloActivas={soloActivas}");
            if (response != null && response.Success && response.Data != null)
            {
                return response.Data;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al consultar zonas: {ex.Message}");
        }

        return new List<ZonaResponseDto>();
    }

    public async Task<bool> CrearAsync(CrearZonaDto dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/Zonas", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear zona: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarZonaDto dto)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"api/Zonas/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar zona {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/Zonas/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar zona {id}: {ex.Message}");
            return false;
        }
    }
}