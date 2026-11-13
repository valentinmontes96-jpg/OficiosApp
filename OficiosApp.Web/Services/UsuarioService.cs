using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using OficiosApp.Shared.Dtos.Base;
using OficiosApp.Shared.Dtos.Profesionales;
using OficiosApp.Shared.Dtos.Usuarios;

namespace OficiosApp.Web.Services;

public class UsuarioService : IUsuarioService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;

    private static readonly ConcurrentDictionary<int, int> _rolesCache = CargarCacheDesdeDisco();
    private static readonly object _fileLock = new();

    private static string RutaArchivoCache => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "OficiosApp",
        "roles_usuarios_cache.json");

    public UsuarioService(HttpClient http, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }

    private static ConcurrentDictionary<int, int> CargarCacheDesdeDisco()
    {
        try
        {
            if (File.Exists(RutaArchivoCache))
            {
                var json = File.ReadAllText(RutaArchivoCache);
                var dict = JsonSerializer.Deserialize<Dictionary<int, int>>(json);
                if (dict != null)
                {
                    return new ConcurrentDictionary<int, int>(dict);
                }
            }
        }
        catch
        {
            // Ignorar errores de lectura de caché local
        }

        return new ConcurrentDictionary<int, int>();
    }

    private static void GuardarRolEnCache(int idUsuario, int idRol)
    {
        if (idUsuario <= 0 || idRol is < 1 or > 3)
            return;

        _rolesCache[idUsuario] = idRol;

        try
        {
            lock (_fileLock)
            {
                var dir = Path.GetDirectoryName(RutaArchivoCache);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var json = JsonSerializer.Serialize(_rolesCache.ToDictionary(k => k.Key, v => v.Value));
                File.WriteAllText(RutaArchivoCache, json);
            }
        }
        catch
        {
            // Ignorar errores de escritura en disco
        }
    }

    private async Task<ClaimsPrincipal?> PrepararHeaderAutenticacionAsync()
    {
        if (_authStateProvider is CustomAuthStateProvider customAuth)
        {
            var token = await customAuth.GetTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        return authState.User;
    }

    public async Task<List<UsuarioDto>> ObtenerTodosAsync()
    {
        try
        {
            var userClaims = await PrepararHeaderAutenticacionAsync();
            var response = await _http.GetFromJsonAsync<BaseResponse<List<UsuarioDto>>>("api/Usuarios");
            if (response != null && response.Success && response.Data != null)
            {
                var usuarios = response.Data;
                await EnriquecerRolesUsuariosAsync(usuarios, userClaims);
                return usuarios;
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
            var userClaims = await PrepararHeaderAutenticacionAsync();
            var response = await _http.GetFromJsonAsync<BaseResponse<UsuarioDto>>($"api/Usuarios/{id}");
            if (response != null && response.Success && response.Data != null)
            {
                var lista = new List<UsuarioDto> { response.Data };
                await EnriquecerRolesUsuariosAsync(lista, userClaims);
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
            await PrepararHeaderAutenticacionAsync();

            // Garantizar que Telefono no viaje vacío para evitar 400 BadRequest si la API valida [Required]
            var payload = new ActualizarUsuarioAdminDto
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? "-" : dto.Telefono.Trim(),
                IdRol = dto.IdRol,
                Activo = dto.Activo
            };

            var response = await _http.PutAsJsonAsync($"api/Usuarios/{id}", payload);
            if (response.IsSuccessStatusCode)
            {
                GuardarRolEnCache(id, dto.IdRol);
                return true;
            }

            return false;
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
            await PrepararHeaderAutenticacionAsync();
            var response = await _http.DeleteAsync($"api/Usuarios/{id}");
            if (response.IsSuccessStatusCode)
            {
                _rolesCache.TryRemove(id, out _);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar usuario {id}: {ex.Message}");
            return false;
        }
    }

    private async Task EnriquecerRolesUsuariosAsync(List<UsuarioDto> usuarios, ClaimsPrincipal? currentUser)
    {
        if (usuarios.Count == 0)
            return;

        // Si el usuario autenticado actual tiene IdRol en su JWT, registrarlo en caché
        int currentUserId = 0;
        int currentUserRol = 0;
        string? currentUserEmail = null;

        if (currentUser?.Identity?.IsAuthenticated == true)
        {
            var idClaim = currentUser.FindFirst("nameid")?.Value
                       ?? currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? currentUser.FindFirst("sub")?.Value;
            int.TryParse(idClaim, out currentUserId);

            var rolClaim = currentUser.FindFirst("IdRol")?.Value
                        ?? currentUser.FindFirst(ClaimTypes.Role)?.Value
                        ?? currentUser.FindFirst("role")?.Value;
            int.TryParse(rolClaim, out currentUserRol);

            currentUserEmail = currentUser.FindFirst("email")?.Value
                            ?? currentUser.FindFirst(ClaimTypes.Email)?.Value;

            if (currentUserId > 0 && currentUserRol is >= 1 and <= 3 && !_rolesCache.ContainsKey(currentUserId))
            {
                GuardarRolEnCache(currentUserId, currentUserRol);
            }
        }

        // Consultar profesionales una sola vez si algún usuario viene sin rol desde la API
        HashSet<int>? idsProfesionales = null;
        bool requiereInferencia = usuarios.Any(u => ResolverIdRolDesdeDto(u) == 0);

        if (requiereInferencia)
        {
            try
            {
                var profResp = await _http.GetFromJsonAsync<BaseResponse<List<ProfesionalResponseDto>>>("api/Profesionales");
                if (profResp?.Success == true && profResp.Data != null)
                {
                    idsProfesionales = profResp.Data.Select(p => p.IdUsuario).ToHashSet();
                }
            }
            catch
            {
                // Ignorar si falla la consulta auxiliar de profesionales
            }
        }

        foreach (var u in usuarios)
        {
            if (u.Telefono == "-")
            {
                u.Telefono = string.Empty;
            }

            int idRol = ResolverIdRolDesdeDto(u);

            // 1. Si el backend no devolvió el rol (ej. versión previa del endpoint remoto), revisar caché de ediciones de Admin
            if (idRol == 0 && _rolesCache.TryGetValue(u.IdUsuario, out var rolCache))
            {
                idRol = rolCache;
            }

            // 2. Si coincide con el usuario logueado actual
            if (idRol == 0 && currentUserRol is >= 1 and <= 3)
            {
                if ((currentUserId > 0 && u.IdUsuario == currentUserId) ||
                    (!string.IsNullOrWhiteSpace(currentUserEmail) &&
                     string.Equals(u.Email, currentUserEmail, StringComparison.OrdinalIgnoreCase)))
                {
                    idRol = currentUserRol;
                    GuardarRolEnCache(u.IdUsuario, idRol);
                }
            }

            // 3. Si está en la tabla de Profesionales
            if (idRol == 0 && idsProfesionales != null && idsProfesionales.Contains(u.IdUsuario))
            {
                idRol = 3;
            }

            // 4. Heurística por correo de administrador si aún no tiene rol
            if (idRol == 0 && !string.IsNullOrWhiteSpace(u.Email) &&
                u.Email.Contains("admin", StringComparison.OrdinalIgnoreCase))
            {
                idRol = 1;
            }

            // 5. Por defecto en el dominio, el rol estándar de registro inicial es Cliente (2)
            if (idRol == 0)
            {
                idRol = 2;
            }

            u.IdRol = idRol;
            u.Rol = ObtenerNombreRol(idRol);
        }
    }

    private static int ResolverIdRolDesdeDto(UsuarioDto u)
    {
        if (u.IdRol is >= 1 and <= 3)
            return u.IdRol;

        if (!string.IsNullOrWhiteSpace(u.Rol))
        {
            var r = u.Rol.Trim();
            if (r == "1" || r.Contains("Admin", StringComparison.OrdinalIgnoreCase))
                return 1;
            if (r == "3" || r.Contains("Pro", StringComparison.OrdinalIgnoreCase))
                return 3;
            if (r == "2" || r.Contains("Cli", StringComparison.OrdinalIgnoreCase))
                return 2;
        }

        return 0;
    }

    private static string ObtenerNombreRol(int idRol) => idRol switch
    {
        1 => "Admin",
        2 => "Cliente",
        3 => "Profesional",
        _ => "Cliente"
    };
}
