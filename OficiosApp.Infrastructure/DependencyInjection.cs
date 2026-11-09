using Microsoft.Extensions.DependencyInjection;
using OficiosApp.Domain.Interfaces;
using OficiosApp.Infrastructure.Repositories;

namespace OficiosApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Registro del Unit of Work y Repositorios
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}