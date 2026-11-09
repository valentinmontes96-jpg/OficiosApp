using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Perfil;
using OficiosApp.Shared.Dtos.Usuarios;
using System.Security.Claims;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("mi-perfil")]
    public async Task<IActionResult> ObtenerMiPerfil()
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _usuarioService.ObtenerMiPerfilAsync(idUsuario);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPut("mi-perfil")]
    public async Task<IActionResult> ActualizarMiPerfil([FromBody] UpdatePerfilRequestDto dto)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _usuarioService.ActualizarMiPerfilAsync(idUsuario, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var resultado = await _usuarioService.ObtenerTodosAsync();
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _usuarioService.ObtenerPorIdAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarUsuarioAdminDto dto)
    {
        var resultado = await _usuarioService.ActualizarAsync(id, dto);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _usuarioService.EliminarAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}