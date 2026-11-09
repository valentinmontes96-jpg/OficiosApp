using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OficiosApp.Application;
using OficiosApp.Infrastructure;
using OficiosApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Base de Datos (PostgreSQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// 4. Configurar Autenticación JWT
var jwtSecret = builder.Configuration["JwtSettings:Secret"] ?? "SuperClaveSecretaDeDesarrollo12345!";
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddControllers();

// 5. Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

app.Run();