using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OficiosApp.Domain.Entities;

namespace OficiosApp.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IConfiguration config)
    {
        // 1. Roles del Sistema
        if (!await context.Roles.AnyAsync())
        {
            var roles = new List<Rol>
            {
                new() { Nombre = "Admin", Descripcion = "Administrador del sistema" },
                new() { Nombre = "Profesional", Descripcion = "Trabajador u oficio verificado" },
                new() { Nombre = "Cliente", Descripcion = "Cliente que contrata servicios" }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        // 2. Estados de Citas / Solicitudes
        if (!await context.EstadosCita.AnyAsync()) // Cambiá EstadosCita por el nombre de tu DbSet si es distinto
        {
            var estados = new List<EstadoCita>
            {
                new() { Nombre = "Pendiente", Descripcion = "Esperando confirmación del profesional" },
                new() { Nombre = "Confirmada", Descripcion = "Aceptada y agendada" },
                new() { Nombre = "Cancelada", Descripcion = "Cancelada por alguna de las partes" },
                new() { Nombre = "Completada", Descripcion = "Trabajo finalizado con éxito" }
            };

            await context.EstadosCita.AddRangeAsync(estados);
            await context.SaveChangesAsync();
        }

        // 3. Categorías de Oficios
        if (!await context.Categorias.AnyAsync())
        {
            var categorias = new List<Categoria>
            {
                new() { Nombre = "Plomería", Descripcion = "Instalaciones de agua, desagües, grifería y sanitarios" },
                new() { Nombre = "Electricidad", Descripcion = "Cableados, tableros eléctricos, tomas e iluminación" },
                new() { Nombre = "Gasista", Descripcion = "Instalaciones de gas matriculado, estufas y calefones" },
                new() { Nombre = "Pintura", Descripcion = "Pintura interior, exterior, impermeabilizaciones" },
                new() { Nombre = "Carpintería", Descripcion = "Muebles a medida, aberturas y arreglos en madera" },
                new() { Nombre = "Albañilería", Descripcion = "Reformas, revoques, colocación de cerámicos y pisos" },
                new() { Nombre = "Climatización", Descripcion = "Instalación y service de aire acondicionado y calefacción" }
            };

            await context.Categorias.AddRangeAsync(categorias);
            await context.SaveChangesAsync();
        }

        // 4. Sembrado Seguro de Administrador
        var adminEmail = config["AdminSeed:Email"];
        var adminPassword = config["AdminSeed:Password"];

        // Si las credenciales no están presentes en la configuración/secretos, no se crea ningún usuario
        if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
        {
            var adminRol = await context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Admin");

            if (adminRol != null && !await context.Usuarios.AnyAsync(u => u.Email == adminEmail))
            {
                var adminUser = new Usuario
                {
                    Email = adminEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword), // Cambiar a 'Clave' o 'Contrasena' según tu entidad
                    Nombre = config["AdminSeed:Nombre"] ?? "Super",
                    Apellido = config["AdminSeed:Apellido"] ?? "Admin",
                    IdRol = adminRol.IdRol,
                    FechaRegistro = DateTime.UtcNow, // Cambiar a 'FechaAlta' o similar si usaste otro
                    Activo = true // Cambiar a 'Estado' si es necesario
                };

                await context.Usuarios.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}