using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Categoria;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerTodas([FromQuery] bool soloActivas = true)
    {
        var resultado = await _categoriaService.ObtenerTodasAsync(soloActivas);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _categoriaService.ObtenerPorIdAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPost]
    [Authorize] // En producción restringir a rol Administrador
    public async Task<IActionResult> Crear([FromBody] CrearCategoriaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var resultado = await _categoriaService.CrearAsync(dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Data!.IdCategoria }, resultado);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCategoriaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var resultado = await _categoriaService.ActualizarAsync(id, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _categoriaService.EliminarAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}