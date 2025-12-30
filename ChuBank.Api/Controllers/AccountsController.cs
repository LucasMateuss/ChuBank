using Asp.Versioning;
using ChuBank.Application.DTOs;
using ChuBank.Application.Interfaces;
using ChuBank.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace ChuBank.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
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
        catch (ChuBankException chuEx)
        {
            return BadRequest(new { error = chuEx.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error: {Message}", ex.Message);
            return StatusCode(500, new { error = "Não foi possível realizar a operação requisitada" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var account = await _accountService.GetByIdAsync(id);

            if (account == null)
            {
                return NotFound(new { message = "Conta não encontrada." });
            }

            return Ok(account);
        }
        catch (ChuBankException chuEx)
        {
            return BadRequest(new { error = chuEx.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error: {Message}", ex.Message);
            return StatusCode(500, new { error = "Não foi possível realizar a operação requisitada" });
        }

    }

    [HttpGet("{id}/statement")]
    public async Task<IActionResult> GetStatement(Guid id, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        try
        {
            var statement = await _accountService.GetStatementAsync(id, start, end);
            return Ok(statement);
        }
        catch (ChuBankException chuEx)
        {
            return BadRequest(new { error = chuEx.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error: {Message}", ex.Message);
            return StatusCode(500, new { error = "Não foi possível realizar a operação requisitada" });
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

    [HttpPost("{id}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid id, [FromBody] AmountDto dto)
    {
        try
        {
            await _accountService.WithdrawAsync(id, dto.Amount);
            return Ok(new { message = "Saque realizado com sucesso." });
        }
        catch (ChuBankException chuEx)
        {
            return BadRequest(new { error = chuEx.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error: {Message}", ex.Message);
            return StatusCode(500, new { error = "Não foi possível realizar a operação requisitada" });
        }
    }

}