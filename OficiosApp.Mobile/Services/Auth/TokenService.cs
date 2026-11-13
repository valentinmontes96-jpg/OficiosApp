using System.Text.Json;

// Services/ITokenStorage.cs
public interface ITokenStorage
{
    Task SetTokenAsync(string token, int? idRol = null, int? idUsuario = null, string? nombreUsuario = null);
    Task<string?> GetTokenAsync();
    int? GetCachedIdRol();
    int? GetCachedIdUsuario();
    string? GetCachedNombreUsuario();
    void RemoveToken();
}

// Services/TokenStorage.cs
public class TokenStorage : ITokenStorage
{
    private const string TokenKey = "auth_token";
    private const string RolKey = "auth_id_rol";
    private const string UsuarioKey = "auth_id_usuario";
    private const string NombreKey = "auth_nombre_usuario";

    private string? _cachedToken;
    private int? _cachedIdRol;
    private int? _cachedIdUsuario;
    private string? _cachedNombreUsuario;

    private static string RutaArchivoFallback =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OficiosApp.Mobile", "auth_session.json");

    public async Task SetTokenAsync(string token, int? idRol = null, int? idUsuario = null, string? nombreUsuario = null)
    {
        _cachedToken = token;
        if (idRol.HasValue && idRol.Value > 0) _cachedIdRol = idRol;
        if (idUsuario.HasValue && idUsuario.Value > 0) _cachedIdUsuario = idUsuario;
        if (!string.IsNullOrWhiteSpace(nombreUsuario)) _cachedNombreUsuario = nombreUsuario;

        try
        {
            await SecureStorage.Default.SetAsync(TokenKey, token);
            if (_cachedIdRol.HasValue)
                await SecureStorage.Default.SetAsync(RolKey, _cachedIdRol.Value.ToString());
            if (_cachedIdUsuario.HasValue)
                await SecureStorage.Default.SetAsync(UsuarioKey, _cachedIdUsuario.Value.ToString());
            if (!string.IsNullOrWhiteSpace(_cachedNombreUsuario))
                await SecureStorage.Default.SetAsync(NombreKey, _cachedNombreUsuario);
        }
        catch
        {
            // En Windows Machine (Unpackaged), SecureStorage lanza excepción por no tener Package Identity.
            // Guardamos en archivo local seguro de la cuenta de usuario.
            try
            {
                var dir = Path.GetDirectoryName(RutaArchivoFallback);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);

                var datos = new SesionLocalDto
                {
                    Token = _cachedToken,
                    IdRol = _cachedIdRol,
                    IdUsuario = _cachedIdUsuario,
                    NombreUsuario = _cachedNombreUsuario
                };
                await File.WriteAllTextAsync(RutaArchivoFallback, JsonSerializer.Serialize(datos));
            }
            catch
            {
            }
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        if (!string.IsNullOrWhiteSpace(_cachedToken))
            return _cachedToken;

        try
        {
            _cachedToken = await SecureStorage.Default.GetAsync(TokenKey);
            if (!string.IsNullOrWhiteSpace(_cachedToken))
                return _cachedToken;
        }
        catch
        {
        }

        try
        {
            if (File.Exists(RutaArchivoFallback))
            {
                var json = await File.ReadAllTextAsync(RutaArchivoFallback);
                var datos = JsonSerializer.Deserialize<SesionLocalDto>(json);
                if (datos != null && !string.IsNullOrWhiteSpace(datos.Token))
                {
                    _cachedToken = datos.Token;
                    _cachedIdRol ??= datos.IdRol;
                    _cachedIdUsuario ??= datos.IdUsuario;
                    _cachedNombreUsuario ??= datos.NombreUsuario;
                }
            }
        }
        catch
        {
        }

        return _cachedToken;
    }

    public int? GetCachedIdRol() => _cachedIdRol;
    public int? GetCachedIdUsuario() => _cachedIdUsuario;
    public string? GetCachedNombreUsuario() => _cachedNombreUsuario;

    public void RemoveToken()
    {
        _cachedToken = null;
        _cachedIdRol = null;
        _cachedIdUsuario = null;
        _cachedNombreUsuario = null;

        try
        {
            SecureStorage.Default.Remove(TokenKey);
            SecureStorage.Default.Remove(RolKey);
            SecureStorage.Default.Remove(UsuarioKey);
            SecureStorage.Default.Remove(NombreKey);
        }
        catch
        {
        }

        try
        {
            if (File.Exists(RutaArchivoFallback))
                File.Delete(RutaArchivoFallback);
        }
        catch
        {
        }
    }

    private sealed class SesionLocalDto
    {
        public string? Token { get; set; }
        public int? IdRol { get; set; }
        public int? IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
    }
}