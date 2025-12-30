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
public class TransfersController : ControllerBase
{
    private readonly ITransferService _transferService;

    public TransfersController(ITransferService transferService)
    {
        _transferService = transferService;
    }

    [HttpPost]
    public async Task<IActionResult> Transfer([FromBody] TransferDto dto)
    {
        try
        {
            await _transferService.PerformTransferAsync(dto);

            return Ok(new { message = "Transferência realizada com sucesso." });
        }
        catch (InvalidOperationException ex) 
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex) 
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno ao processar transferência.", details = ex.Message });
        }
    }
}