using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Clientes;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Todo protegido, la información de clientes suele ser privada
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var resultado = await _clienteService.ObtenerPorIdAsync(id);
        if (!resultado.Success)
            return NotFound(resultado);

        return Ok(resultado);
    }

    [HttpPut("mi-perfil-cliente")]
    public async Task<IActionResult> ActualizarMiPerfil([FromBody] ActualizarClienteDto dto)
    {
        var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            return Unauthorized("Token no válido.");

        var resultado = await _clienteService.ActualizarPerfilAsync(idUsuario, dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }
}