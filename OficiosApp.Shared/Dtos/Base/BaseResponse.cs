namespace OficiosApp.Shared.Dtos.Base;

public class BaseResponse<T>
{
    public bool Success { get; set; } = true;
    public string ErrorMessage { get; set; } = string.Empty;
    public T? Data { get; set; }
}