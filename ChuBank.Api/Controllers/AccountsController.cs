using Asp.Versioning;
using ChuBank.Application.DTOs;
using ChuBank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChuBank.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
    {
        try
        {
            var account = await _accountService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var account = await _accountService.GetByIdAsync(id);

        if (account == null)
        {
            return NotFound(new { message = "Conta não encontrada." });
        }

        return Ok(account);
    }

    [HttpGet("{id}/statement")]
    public async Task<IActionResult> GetStatement(Guid id, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        try
        {
            var statement = await _accountService.GetStatementAsync(id, start, end);
            return Ok(statement);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    [HttpPost("{id}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, [FromBody] AmountDto dto)
    {
        try
        {
            await _accountService.DepositAsync(id, dto.Amount);
            return Ok(new { message = "Depósito realizado com sucesso." });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Conta não encontrada." });
        }
        catch (ArgumentException ex) 
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("{id}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid id, [FromBody] AmountDto dto)
    {
        try
        {
            await _accountService.WithdrawAsync(id, dto.Amount);
            return Ok(new { message = "Saque realizado com sucesso." });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Conta não encontrada." });
        }
        catch (InvalidOperationException ex) 
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex) 
        {
            return BadRequest(new { error = ex.Message });
        }
    }

}