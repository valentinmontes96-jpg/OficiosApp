using Microsoft.Extensions.DependencyInjection;
using OficiosApp.Application.Interfaces;
using OficiosApp.Application.Services;

namespace OficiosApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registro de los servicios de negocio
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICitaService, CitaService>();

        return services;
    }
}