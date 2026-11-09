using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Reportes;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportesUsuariosController : ControllerBase
{
    private readonly IReporteUsuarioService _reporteService;

    public ReportesUsuariosController(IReporteUsuarioService reporteService)
    {
        _reporteService = reporteService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos([FromQuery] string? estado)
    {
        var resultado = await _reporteService.ObtenerTodosAsync(estado);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _reporteService.ObtenerPorIdAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearReporteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idDenunciante))
            return Unauthorized("Token no válido.");

        var resultado = await _reporteService.CrearAsync(idDenunciante, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Data!.IdReporte }, resultado);
    }

    [HttpPatch("{id:int}/resolver")]
    public async Task<IActionResult> Resolver(int id, [FromBody] ResolverReporteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idAdmin))
            return Unauthorized("Token no válido.");

        var resultado = await _reporteService.ResolverAsync(id, idAdmin, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }
}