using System.Net.Http.Headers;

namespace OficiosApp.Mobile.Services.Auth;

public class MobileAuthHeaderHandler : DelegatingHandler
{
    private readonly ITokenStorage _tokenStorage;

    public MobileAuthHeaderHandler(ITokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // 1. Obtiene el token guardado de forma segura en el dispositivo
        var token = await _tokenStorage.GetTokenAsync();

        // 2. Si existe un token, lo adjunta en la cabecera Authorization: Bearer <token>
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // 3. Pasa la petición al siguiente eslabón del pipeline HTTP
        return await base.SendAsync(request, cancellationToken);
    }
}