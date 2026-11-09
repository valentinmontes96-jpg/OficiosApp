using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Profesionales;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfesionalesController : ControllerBase
{
    private readonly IProfesionalService _profesionalService;

    public ProfesionalesController(IProfesionalService profesionalService)
    {
        _profesionalService = profesionalService;
    }

    [HttpGet]
    [AllowAnonymous] // Público: Cualquiera puede ver la lista de profesionales
    public async Task<IActionResult> ObtenerTodos()
    {
        var resultado = await _profesionalService.ObtenerTodosAsync();
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous] // Público: Ver detalle de un profesional
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _profesionalService.ObtenerPorIdAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPut("mi-perfil-profesional")]
    [Authorize] // Protegido: Solo el dueño edita su perfil
    public async Task<IActionResult> ActualizarMiPerfil([FromBody] ActualizarProfesionalDto dto)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _profesionalService.ActualizarPerfilAsync(idUsuario, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPut("mis-categorias")]
    [Authorize]
    public async Task<IActionResult> AsignarCategorias([FromBody] List<int> idsCategorias)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized();

        var resultado = await _profesionalService.AsignarCategoriasAsync(idUsuario, idsCategorias);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPut("mis-zonas")]
    [Authorize]
    public async Task<IActionResult> AsignarZonas([FromBody] List<int> idsZonas)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized();

        var resultado = await _profesionalService.AsignarZonasAsync(idUsuario, idsZonas);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }
}