using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OficiosApp.Application.Interfaces;
using OficiosApp.Domain.Entities;

namespace OficiosApp.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerarToken(Usuario usuario)
    {
        var secretKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("La clave secreta 'Jwt:Key' no está configurada.");
        var issuer = _configuration["Jwt:Issuer"] ?? "OficiosApp";
        var audience = _configuration["Jwt:Audience"] ?? "OficiosApp";
        var expiracionHoras = double.TryParse(_configuration["Jwt:ExpiracionHoras"], out var horas) ? horas : 24;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}".Trim()),
            new("IdRol", usuario.IdRol.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(expiracionHoras),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}