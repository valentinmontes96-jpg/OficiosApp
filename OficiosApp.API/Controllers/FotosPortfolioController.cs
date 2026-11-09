using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Portfolio;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FotosPortfolioController : ControllerBase
{
    private readonly IFotoPortfolioService _portfolioService;

    public FotosPortfolioController(IFotoPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    [HttpGet("profesional/{idProfesional:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorProfesional(int idProfesional)
    {
        var resultado = await _portfolioService.ObtenerPorProfesionalAsync(idProfesional);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _portfolioService.ObtenerPorIdAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Agregar([FromBody] CrearFotoPortfolioDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _portfolioService.AgregarAsync(idUsuario, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Data!.IdFoto }, resultado);
    }

    [HttpPatch("{id:int}")]
    [Authorize]
    public async Task<IActionResult> ActualizarDescripcion(int id, [FromBody] ActualizarFotoPortfolioDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _portfolioService.ActualizarDescripcionAsync(id, idUsuario, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Eliminar(int id)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _portfolioService.EliminarAsync(id, idUsuario);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}