using OficiosApp.Mobile;
using OficiosApp.Mobile.Services;
using OficiosApp.Mobile.Services.Auth;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Habilita Blazor WebView nativo
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        // Servicios de almacenamiento y autenticación
        builder.Services.AddSingleton<ITokenStorage, TokenStorage>();
        builder.Services.AddTransient<MobileAuthHeaderHandler>();

        // Cliente HTTP apuntando a tu API en producción
        builder.Services.AddHttpClient("OficiosApi", client =>
        {
            client.BaseAddress = new Uri("https://oficiosapi.runasp.net/");
        })
        .AddHttpMessageHandler<MobileAuthHeaderHandler>();

        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>().CreateClient("OficiosApi"));

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ICategoriaService, CategoriaService>();
        builder.Services.AddScoped<IProfesionalMobileService, ProfesionalMobileService>();
        builder.Services.AddScoped<ICitaMobileService, CitaMobileService>();


        return builder.Build();
    }
}