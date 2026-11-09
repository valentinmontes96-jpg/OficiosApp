using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstadosCitaController : ControllerBase
{
    private readonly IEstadoCitaService _estadoCitaService;

    public EstadosCitaController(IEstadoCitaService estadoCitaService)
    {
        _estadoCitaService = estadoCitaService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerTodos()
    {
        var resultado = await _estadoCitaService.ObtenerTodosAsync();
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _estadoCitaService.ObtenerPorIdAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }
}