using System.Net.Http.Json;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.HorariosAtencion;
using OficiosApp.Shared.Dtos.Profesionales;
using OficiosApp.Shared.Dtos.Zonas;

namespace OficiosApp.Mobile.Services;

public class ProfesionalMobileService : IProfesionalMobileService
{
    private readonly HttpClient _httpClient;

    public ProfesionalMobileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProfesionalResponseDto>> ObtenerTodosAsync()
    {
        try
        {
            var respuesta = await _httpClient.GetFromJsonAsync<BaseResponse<List<ProfesionalResponseDto>>>("api/profesionales");
            if (respuesta != null && respuesta.Success && respuesta.Data != null)
            {
                return respuesta.Data;
            }
            return new List<ProfesionalResponseDto>();
        }
        catch
        {
            return new List<ProfesionalResponseDto>();
        }
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

    public async Task<bool> AsignarCategoriasAsync(List<int> idsCategorias)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/profesionales/mis-categorias", idsCategorias);
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

    public async Task<bool> AsignarZonasAsync(List<int> idsZonas)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/profesionales/mis-zonas", idsZonas);
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

    public async Task<List<ZonaResponseDto>> ObtenerZonasAsync()
    {
        try
        {
            var respuesta = await _httpClient.GetFromJsonAsync<BaseResponse<List<ZonaResponseDto>>>("api/zonas?soloActivas=true");
            if (respuesta != null && respuesta.Success && respuesta.Data != null)
            {
                return respuesta.Data;
            }
            return new List<ZonaResponseDto>();
        }
        catch
        {
            return new List<ZonaResponseDto>();
        }
    }

    public async Task<List<HorarioAtencionResponseDto>> ObtenerHorariosAsync(int idProfesional)
    {
        try
        {
            var respuesta = await _httpClient.GetFromJsonAsync<BaseResponse<List<HorarioAtencionResponseDto>>>($"api/horariosatencion/profesional/{idProfesional}");
            if (respuesta != null && respuesta.Success && respuesta.Data != null)
            {
                return respuesta.Data.OrderBy(h => h.DiaSemana).ToList();
            }
            return new List<HorarioAtencionResponseDto>();
        }
        catch
        {
            return new List<HorarioAtencionResponseDto>();
        }
    }

    public async Task<bool> SincronizarHorariosAsync(List<CrearOActualizarHorarioDto> horarios)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/horariosatencion/sincronizar-semana", horarios);
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
