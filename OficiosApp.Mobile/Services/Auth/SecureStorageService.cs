namespace OficiosApp.Mobile.Services.Auth;

public static class SecureStorageService
{
    public static async Task GuardarTokenAsync(string token)
    {
        await SecureStorage.Default.SetAsync("jwt_token", token);
    }

    public static async Task<string?> ObtenerTokenAsync()
    {
        return await SecureStorage.Default.GetAsync("jwt_token");
    }

    public static void BorrarToken()
    {
        SecureStorage.Default.Remove("jwt_token");
    }
}