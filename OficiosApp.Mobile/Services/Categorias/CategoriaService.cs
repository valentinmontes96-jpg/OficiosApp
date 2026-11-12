using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Categoria;
using System.Net.Http.Json;

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
            var respuesta = await _httpClient.GetFromJsonAsync<BaseResponse<List<CategoriaResponseDto>>>("api/categorias");

            if (respuesta != null && respuesta.Success && respuesta.Data != null)
            {
                return respuesta.Data.Where(c => c.Activa).ToList();
            }

            return new List<CategoriaResponseDto>();
        }
        catch
        {
            return new List<CategoriaResponseDto>();
        }
    }
}