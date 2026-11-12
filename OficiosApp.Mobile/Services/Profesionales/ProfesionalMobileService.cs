using System.Net.Http.Json;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Profesionales;

namespace OficiosApp.Mobile.Services;

public class ProfesionalMobileService : IProfesionalMobileService
{
    private readonly HttpClient _httpClient;

    public ProfesionalMobileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProfesionalResponseDto?> ObtenerPerfilAsync(int idUsuario)
    {
        try
        {
            var respuesta = await _httpClient.GetFromJsonAsync<BaseResponse<ProfesionalResponseDto>>($"api/profesionales/{idUsuario}");
            if (respuesta != null && respuesta.Success)
            {
                return respuesta.Data;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<ProfesionalResponseDto?> ActualizarPerfilAsync(ActualizarProfesionalDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/profesionales/mi-perfil-profesional", dto);
            if (!response.IsSuccessStatusCode)
                return null;

            var resultado = await response.Content.ReadFromJsonAsync<BaseResponse<ProfesionalResponseDto>>();
            return resultado != null && resultado.Success ? resultado.Data : null;
        }
        catch
        {
            return null;
        }
    }
}

