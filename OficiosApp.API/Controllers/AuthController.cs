using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosApp.Application.Interfaces;
using OficiosApp.Shared.Dtos.Auth;

namespace OficiosApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar([FromBody] RegistroRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var resultado = await _authService.RegistrarAsync(dto);
        if (!resultado.Success)
            return BadRequest(resultado);

        return Ok(resultado);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var resultado = await _authService.LoginAsync(dto);
        if (!resultado.Success)
            return Unauthorized(resultado);

        return Ok(resultado);
    }
}