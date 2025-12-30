using ChuBank.Application.DTOs;
using ChuBank.Application.Interfaces;
using ChuBank.Domain.Exceptions;
using ChuBank.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace ChuBank.Application.Services;

public class TransferService : ITransferService
{
    private readonly IAccountRepository _repository;
    private readonly IHolidayService _holidayService;

    public TransferService(IAccountRepository repository, IHolidayService holidayService)
    {
        _repository = repository;
        _holidayService = holidayService;
    }

    public async Task PerformTransferAsync(TransferDto dto)
    {
        var isBusinessDay = await _holidayService.IsBusinessDayAsync(DateTime.UtcNow);
        if (!isBusinessDay)
        {
            throw new ChuBankException("Transferências só podem ser feitas nos dias úteis.");
        }

        var fromAccount = await _repository.GetByIdAsync(dto.FromAccountId);
        var toAccount = await _repository.GetByIdAsync(dto.ToAccountId);

        if (fromAccount == null || toAccount == null)
        {
            throw new ChuBankException("Conta de origem ou destino inválida.");
        }

        fromAccount.Withdraw(dto.Amount, toAccountId: toAccount.Id);
        toAccount.Deposit(dto.Amount, fromAccountId: fromAccount.Id);

        await _repository.UpdateAsync(fromAccount);
        await _repository.UpdateAsync(toAccount);
    }
}