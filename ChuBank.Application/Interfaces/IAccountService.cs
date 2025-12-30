using ChuBank.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Application.Interfaces;

public interface IAccountService
{
    Task<AccountResponseDto> CreateAsync(CreateAccountDto dto);
    Task<AccountResponseDto?> GetByIdAsync(Guid id);
    Task<AccountStatementDto> GetStatementAsync(Guid accountId, DateTime? start = null, DateTime? end = null);
    Task<string> LoginAsync(LoginDto dto);

    // Métodos para teste
    Task DepositAsync(Guid accountId, decimal amount);
    Task WithdrawAsync(Guid accountId, decimal amount);
}
