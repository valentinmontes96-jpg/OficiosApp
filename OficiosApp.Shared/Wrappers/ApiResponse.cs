namespace OficiosApp.Shared.Wrappers;

//Esto garantiza que tu API siempre responda con la misma estructura, lo que hace que consumirla desde Angular,
//Blazor o MAUI sea predecible y no rompa la aplicación si hay un error.
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    // Constructores para facilitar su uso en los Servicios
    public ApiResponse() { }

    public ApiResponse(T data, string message = "")
    {
        Success = true;
        Message = message;
        Data = data;
    }

    public ApiResponse(string message, List<string>? errors = null)
    {
        Success = false;
        Message = message;
        Errors = errors;
    }
}