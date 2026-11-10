using System.Net.Http.Json;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Categoria;

namespace OficiosApp.Web.Services;

public class CategoriaService : ICategoriaService
{
    private readonly HttpClient _http;

    public CategoriaService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<CategoriaResponseDto>> ObtenerTodasAsync(bool soloActivas = false)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<BaseResponse<List<CategoriaResponseDto>>>($"api/Categorias?soloActivas={soloActivas}");
            if (response != null && response.Success && response.Data != null)
            {
                return response.Data;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener categorías: {ex.Message}");
        }

        return new List<CategoriaResponseDto>();
    }

    public async Task<bool> CrearAsync(CrearCategoriaDto dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/Categorias", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear categoría: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarCategoriaDto dto)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"api/Categorias/{id}", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar categoría: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/Categorias/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar categoría: {ex.Message}");
            return false;
        }
    }
}