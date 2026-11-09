using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Favoritos;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoritosController : ControllerBase
{
    private readonly IFavoritoService _favoritoService;

    public FavoritosController(IFavoritoService favoritoService)
    {
        _favoritoService = favoritoService;
    }

    [HttpGet("mis-favoritos")]
    public async Task<IActionResult> ObtenerMisFavoritos()
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuarioCliente))
            return Unauthorized("Token no válido.");

        var resultado = await _favoritoService.ObtenerMisFavoritosAsync(idUsuarioCliente);
        return Ok(resultado);
    }

    [HttpGet("comprobar/{idProfesional:int}")]
    public async Task<IActionResult> ComprobarFavorito(int idProfesional)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuarioCliente))
            return Unauthorized("Token no válido.");

        var resultado = await _favoritoService.EsFavoritoAsync(idUsuarioCliente, idProfesional);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] AgregarFavoritoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuarioCliente))
            return Unauthorized("Token no válido.");

        var resultado = await _favoritoService.AgregarAsync(idUsuarioCliente, dto.IdUsuarioProfesional);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{idProfesional:int}")]
    public async Task<IActionResult> Eliminar(int idProfesional)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuarioCliente))
            return Unauthorized("Token no válido.");

        var resultado = await _favoritoService.EliminarAsync(idUsuarioCliente, idProfesional);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}