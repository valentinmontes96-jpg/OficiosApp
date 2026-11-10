using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace OficiosApp.Web.Services;

/// <summary>
/// Handler neutro que satisface los requisitos de IAuthenticationService 
/// en Blazor Server sin interferir con ProtectedLocalStorage.
/// </summary>
public class CustomAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public CustomAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // En Blazor Server interactivo, la autenticación real 
        // la gestiona CustomAuthStateProvider vía SignalR.
        return Task.FromResult(AuthenticateResult.NoResult());
    }
}