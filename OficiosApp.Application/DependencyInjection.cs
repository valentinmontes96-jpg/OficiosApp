using Microsoft.Extensions.DependencyInjection;
using OficiosApp.Application.Interfaces;
using OficiosApp.Application.Services;
using OficiosApp.Infrastructure.Services;

namespace OficiosApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registro de los servicios de negocio
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICitaService, CitaService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IProfesionalService, ProfesionalService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IResenaService, ResenaService>();
        services.AddScoped<IZonaService, ZonaService>();
        services.AddScoped<IHorarioAtencionService, HorarioAtencionService>();
        services.AddScoped<IFotoPortfolioService, FotoPortfolioService>();
        services.AddScoped<INotificacionService, NotificacionService>();
        services.AddScoped<IReporteUsuarioService, ReporteUsuarioService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IEstadoCitaService, EstadoCitaService>();

        return services;
    }
}