using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Zonas;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZonasController : ControllerBase
{
    private readonly IZonaService _zonaService;

    public ZonasController(IZonaService zonaService)
    {
        _zonaService = zonaService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerTodas([FromQuery] bool soloActivas = true)
    {
        var resultado = await _zonaService.ObtenerTodasAsync(soloActivas);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _zonaService.ObtenerPorIdAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPost]
    [Authorize] // En producción podés acotarlo a roles de administración
    public async Task<IActionResult> Crear([FromBody] CrearZonaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var resultado = await _zonaService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Data!.IdZona }, resultado);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarZonaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var resultado = await _zonaService.ActualizarAsync(id, dto);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _zonaService.EliminarAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}