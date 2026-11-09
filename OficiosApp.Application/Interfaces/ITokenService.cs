using OficiosApp.Domain.Entities;

namespace OficiosApp.Application.Interfaces;

public interface ITokenService
{
    string GenerarToken(Usuario usuario);
}