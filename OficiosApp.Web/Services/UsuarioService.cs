using System.Net.Http.Json;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Usuarios;

namespace OficiosApp.Web.Services;

public class UsuarioService : IUsuarioService
{
    private readonly HttpClient _http;

    public UsuarioService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<UsuarioDto>> ObtenerTodosAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<BaseResponse<List<UsuarioDto>>>("api/Usuarios");
            if (response != null && response.Success && response.Data != null)
            {
                return response.Data;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al consultar usuarios: {ex.Message}");
        }

        return new List<UsuarioDto>();
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<BaseResponse<UsuarioDto>>($"api/Usuarios/{id}");
            if (response != null && response.Success)
            {
                return response.Data;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al consultar usuario {id}: {ex.Message}");
        }

        return null;
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarUsuarioAdminDto dto)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"api/Usuarios/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar usuario {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/Usuarios/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar usuario {id}: {ex.Message}");
            return false;
        }
    }
}