using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using OficiosApp.Web.Components;
using OficiosApp.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication("CustomScheme")
    .AddScheme<AuthenticationSchemeOptions, CustomAuthHandler>("CustomScheme", _ => { });

builder.Services.AddScoped<AuthHeaderHandler>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddHttpClient("OficiosApi", client =>
{
    client.BaseAddress = new Uri("https://oficiosapi.runasp.net/");
})
.AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("OficiosApi"));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProfesionalService, ProfesionalService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IZonaService, ZonaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Asegurar soporte estándar para archivos estáticos en wwwroot en IIS
app.UseStaticFiles();

// Middleware explícito de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();