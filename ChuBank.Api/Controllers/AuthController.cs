using Asp.Versioning;
using ChuBank.Application.DTOs;
using ChuBank.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChuBank.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AuthController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var token = await _accountService.LoginAsync(dto);
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Conta ou senha inválidos." });
        }
    }
}