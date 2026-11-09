using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Citas;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CitasController : ControllerBase
{
    private readonly ICitaService _citaService;

    public CitasController(ICitaService citaService)
    {
        _citaService = citaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodas()
    {
        var resultado = await _citaService.ObtenerTodasAsync();
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _citaService.ObtenerPorIdAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpGet("mis-citas-cliente")]
    public async Task<IActionResult> ObtenerMisCitasCliente()
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _citaService.ObtenerPorClienteAsync(idUsuario);
        return Ok(resultado);
    }

    [HttpGet("mis-citas-profesional")]
    public async Task<IActionResult> ObtenerMisCitasProfesional()
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _citaService.ObtenerPorProfesionalAsync(idUsuario);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearCitaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuarioCliente))
            return Unauthorized("Token no válido.");

        var resultado = await _citaService.CrearAsync(idUsuarioCliente, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Data!.IdCita }, resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCitaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var resultado = await _citaService.ActualizarAsync(id, dto);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoCitaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuarioAccion))
            return Unauthorized("Token no válido.");

        var resultado = await _citaService.CambiarEstadoAsync(id, idUsuarioAccion, dto.NuevoEstadoId, dto.Observaciones);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpGet("{id:int}/historial")]
    public async Task<IActionResult> ObtenerHistorial(int id)
    {
        var resultado = await _citaService.ObtenerHistorialAsync(id);
        return Ok(resultado);
    }
}