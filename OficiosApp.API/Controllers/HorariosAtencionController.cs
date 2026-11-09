using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.HorariosAtencion;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HorariosAtencionController : ControllerBase
{
    private readonly IHorarioAtencionService _horarioService;

    public HorariosAtencionController(IHorarioAtencionService horarioService)
    {
        _horarioService = horarioService;
    }

    [HttpGet("profesional/{idProfesional:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorProfesional(int idProfesional)
    {
        var resultado = await _horarioService.ObtenerPorProfesionalAsync(idProfesional);
        return Ok(resultado);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> GuardarDia([FromBody] CrearOActualizarHorarioDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _horarioService.GuardarDiaAsync(idUsuario, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPut("sincronizar-semana")]
    [Authorize]
    public async Task<IActionResult> SincronizarSemana([FromBody] List<CrearOActualizarHorarioDto> listaHorarios)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _horarioService.SincronizarSemanaAsync(idUsuario, listaHorarios);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{idHorario:int}")]
    [Authorize]
    public async Task<IActionResult> Eliminar(int idHorario)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _horarioService.EliminarAsync(idHorario, idUsuario);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}