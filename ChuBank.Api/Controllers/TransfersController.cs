using Asp.Versioning;
using ChuBank.Application.DTOs;
using ChuBank.Application.Interfaces;
using ChuBank.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChuBank.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")] 
[Route("api/v{version:apiVersion}/[controller]")]
public class TransfersController : ControllerBase
{
    private readonly ITransferService _transferService;
    private readonly ILogger<AccountsController> _logger;

    public TransfersController(ITransferService transferService, ILogger<AccountsController> logger)
    {
        _transferService = transferService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Transfer([FromBody] TransferDto dto)
    {
        try
        {
            await _transferService.PerformTransferAsync(dto);

            return Ok(new { message = "Transferência realizada com sucesso." });
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