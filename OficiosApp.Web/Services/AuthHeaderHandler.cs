using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace OficiosApp.Web.Services;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthHeaderHandler(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_authStateProvider is CustomAuthStateProvider customAuth)
        {
            var token = await customAuth.GetTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}