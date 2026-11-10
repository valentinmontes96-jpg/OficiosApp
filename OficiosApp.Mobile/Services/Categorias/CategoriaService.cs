using System.Net.Http.Json;
using OficiosApp.Shared.Dtos.Categoria;

namespace OficiosApp.Mobile.Services;

public class CategoriaService : ICategoriaService
{
    private readonly HttpClient _httpClient;

    public CategoriaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CategoriaResponseDto>> ObtenerCategoriasAsync()
    {
        try
        {
            var respuesta = await _httpClient.GetFromJsonAsync<List<CategoriaResponseDto>>("api/categorias");
            return respuesta?.Where(c => c.Activa).ToList() ?? new List<CategoriaResponseDto>();
        }
        catch
        {
            return new List<CategoriaResponseDto>();
        }
    }
}