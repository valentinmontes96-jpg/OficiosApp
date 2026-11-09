using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting; // Necesario para atrapar HostAbortedException
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OficiosApp.Application;
using OficiosApp.Infrastructure;
using OficiosApp.Infrastructure.Data;
using Serilog;
using System.Text;
using OficiosApp.Domain.Entities;

// Configuración inicial de Serilog para registrar eventos antes de levantar el host
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando OficiosApp API...");

    var builder = WebApplication.CreateBuilder(args);

    // Forzar la lectura de User Secrets en cualquier entorno
    builder.Configuration.AddUserSecrets<Program>();

    // Integración de Serilog reemplazando el logger por defecto
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // 1. Configurar DbContext tomando la cadena de conexión de secrets.json (o variables de entorno)
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Cadena de conexión 'DefaultConnection' no encontrada en la configuración.")));

    // 2. Inyectar dependencias de nuestras capas
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices();

    // 3. Configurar CORS (Permitir peticiones de Blazor Web y MAUI Mobile)
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("PermitirTodo", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // 4. Configurar Autenticación JWT (Leemos de la clave "Jwt:Key" del secrets.json)
    var jwtSecret = builder.Configuration["Jwt:Key"] ?? "SuperClaveSecretaDeDesarrollo12345!";
    var key = Encoding.ASCII.GetBytes(jwtSecret);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // En producción podrías habilitarlo y validar el "Issuer"
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true
            };
        });

    builder.Services.AddControllers();

    // 5. Configurar Swagger con JWT
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "OficiosApp API", Version = "v1" });

        var scheme = new OpenApiSecurityScheme
        {
            Description = "Autorización JWT usando Bearer. Formato: 'Bearer {token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        };

        options.AddSecurityDefinition("Bearer", scheme);

        // Se pasa la expresión lambda (doc => ...) que requiere la firma actual
        options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer"),
                new List<string>()
            }
        });
    });



    var app = builder.Build();

    // Middleware de Serilog para loguear peticiones HTTP resumidas
    app.UseSerilogRequestLogging();

    // Atrapa todas las excepciones y devuelve JSON limpio
    app.UseMiddleware<OficiosApp.API.Middlewares.ExceptionMiddleware>();

    // 6. Configurar el pipeline HTTP
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("PermitirTodo");
    app.UseHttpsRedirection();

    // El orden de estos dos es crítico
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // 7. Ejecutar Data Seeding en el arranque
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var config = services.GetRequiredService<IConfiguration>();

            // Llamada al seeder que creamos en Infrastructure
            await DataSeeder.SeedAsync(context, config);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ocurrió un error al ejecutar el sembrado inicial de datos (Data Seeding).");
        }
    }

    app.Run();
}
catch (HostAbortedException)
{
    // Silenciar: esto previene que herramientas de línea de comandos (como dotnet ef) tiren errores fatales al construir el host
    throw;
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar.");
}
finally
{
    Log.CloseAndFlush();
}