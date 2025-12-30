using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuBank.Application.DTOs;
using ChuBank.Application.Interfaces;
using ChuBank.Domain.Entities;
using ChuBank.Domain.Interfaces;
using ChuBank.Domain.Exceptions;

namespace ChuBank.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    private readonly ITokenService _tokenService;

    public AccountService(IAccountRepository repository, ITokenService tokenService)
    {
        _repository = repository;
        _tokenService = tokenService;
    }

    public async Task<AccountResponseDto> CreateAsync(CreateAccountDto dto)
    {
        var existingAccount = await _repository.GetByNumberAsync(dto.Number);
        if (existingAccount != null)
        {
            throw new ChuBankException("Já existe uma conta com esse número.");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var account = new Account(dto.Number, dto.Holder, passwordHash);

        await _repository.AddAsync(account);

        return new AccountResponseDto(account.Id, account.Number, account.Holder, account.Balance);
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var account = await _repository.GetByNumberAsync(dto.AccountNumber);

        if (account == null || !BCrypt.Net.BCrypt.Verify(dto.Password, account.PasswordHash))
        {
            throw new ChuBankException("Credenciais inválidas.");
        }

        return _tokenService.GenerateToken(account);
    }

    public async Task<AccountResponseDto?> GetByIdAsync(Guid id)
    {
        var account = await _repository.GetByIdAsync(id);
        if (account == null)
        {
            return null;
        }

        return new AccountResponseDto(account.Id, account.Number, account.Holder, account.Balance);
    }

    public async Task<AccountStatementDto> GetStatementAsync(Guid accountId, DateTime? start = null, DateTime? end = null)
    {
        var account = await _repository.GetByIdAsync(accountId);
        if (account == null)
        {
            throw new ChuBankException("Conta não encontrada.");
        }

        var transactions = await _repository.GetTransactionsByAccountIdAsync(accountId, start, end);

        var statement = new AccountStatementDto
        {
            AccountNumber = account.Number,
            Holder = account.Holder,
            CurrentBalance = account.Balance
        };

        foreach (var t in transactions)
        {
            bool isDebit = t.Type == TransactionType.Debit;
            string description;

            if (isDebit)
            {
                description = t.ToAccountId == null
                    ? "Saque realizado"
                    : $"Transferência enviada para {t.ToAccountId}";
            }
            else
            {
                description = t.FromAccountId == null
                    ? "Depósito recebido"
                    : $"Transferência recebida de {t.FromAccountId}";
            }

            statement.Transactions.Add(new TransactionItemDto
            {
                Date = t.TransactionDate,
                Amount = t.Amount,
                Type = isDebit ? "Débito" : "Crédito",
                Description = description
            });
        }

        return statement;
    }

    // Métodos para teste
    public async Task DepositAsync(Guid accountId, decimal amount)
    {
        var account = await _repository.GetByIdAsync(accountId);
        if (account == null)
        {
            throw new ChuBankException("Conta não encontrada.");
        }

        account.Deposit(amount);

        await _repository.UpdateAsync(account);
    }

    public async Task WithdrawAsync(Guid accountId, decimal amount)
    {
        var account = await _repository.GetByIdAsync(accountId);
        if (account == null)
        {
            throw new ChuBankException("Conta não encontrada.");
        }

        account.Withdraw(amount);

        await _repository.UpdateAsync(account);
    }
}