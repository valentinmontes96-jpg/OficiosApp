using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificacionesController : ControllerBase
{
    private readonly INotificacionService _notificacionService;

    public NotificacionesController(INotificacionService notificacionService)
    {
        _notificacionService = notificacionService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerMisNotificaciones([FromQuery] bool soloNoLeidas = false)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _notificacionService.ObtenerPorUsuarioAsync(idUsuario, soloNoLeidas);
        return Ok(resultado);
    }

    [HttpGet("contador-no-leidas")]
    public async Task<IActionResult> ContarNoLeidas()
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _notificacionService.ContarNoLeidasAsync(idUsuario);
        return Ok(resultado);
    }

    [HttpPatch("{id:int}/leer")]
    public async Task<IActionResult> MarcarComoLeida(int id)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _notificacionService.MarcarComoLeidaAsync(id, idUsuario);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPatch("leer-todas")]
    public async Task<IActionResult> MarcarTodasComoLeidas()
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _notificacionService.MarcarTodasComoLeidasAsync(idUsuario);
        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _notificacionService.EliminarAsync(id, idUsuario);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}