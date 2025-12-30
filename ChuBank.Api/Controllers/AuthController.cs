using Asp.Versioning;
using ChuBank.Application.DTOs;
using ChuBank.Application.Interfaces;
using ChuBank.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ChuBank.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AuthController(IAccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var token = await _accountService.LoginAsync(dto);
            return Ok(new { token });
        }
        catch (ChuBankException chuEx)
        {
            return BadRequest(new { error = chuEx.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error: {Message}", ex.Message);
            return StatusCode(500, new { error = "Não foi possível realizar a operação requisitada" });
        }
    }
}