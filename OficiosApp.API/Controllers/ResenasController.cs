using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Resenas;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResenasController : ControllerBase
{
    private readonly IResenaService _resenaService;

    public ResenasController(IResenaService resenaService)
    {
        _resenaService = resenaService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerTodas()
    {
        var resultado = await _resenaService.ObtenerTodasAsync();
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _resenaService.ObtenerPorIdAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpGet("cita/{idCita:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorCita(int idCita)
    {
        var resultado = await _resenaService.ObtenerPorCitaAsync(idCita);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpGet("profesional/{idProfesional:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorProfesional(int idProfesional)
    {
        var resultado = await _resenaService.ObtenerPorProfesionalAsync(idProfesional);
        return Ok(resultado);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Crear([FromBody] CrearResenaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuarioCliente))
            return Unauthorized("Token no válido.");

        var resultado = await _resenaService.CrearAsync(idUsuarioCliente, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Data!.IdResena }, resultado);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _resenaService.EliminarAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}