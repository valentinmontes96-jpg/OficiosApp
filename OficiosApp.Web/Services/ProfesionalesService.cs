using System.Net.Http.Json;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Profesionales;

namespace OficiosApp.Web.Services;

public class ProfesionalService : IProfesionalService
{
    private readonly HttpClient _http;

    public ProfesionalService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ProfesionalResponseDto>> ObtenerTodosAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<BaseResponse<List<ProfesionalResponseDto>>>("api/Profesionales");
            if (response != null && response.Success && response.Data != null)
            {
                return response.Data;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al consultar profesionales: {ex.Message}");
        }

        return new List<ProfesionalResponseDto>();
    }

    public async Task<ProfesionalResponseDto?> ObtenerPorIdAsync(int id)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<BaseResponse<ProfesionalResponseDto>>($"api/Profesionales/{id}");
            if (response != null && response.Success)
            {
                return response.Data;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al consultar detalle del profesional {id}: {ex.Message}");
        }

        return null;
    }
}