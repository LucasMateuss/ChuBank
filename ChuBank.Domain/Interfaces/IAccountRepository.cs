using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuBank.Domain.Entities;

namespace ChuBank.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);

    Task<Account?> GetByNumberAsync(string number);

    Task AddAsync(Account account);

    Task UpdateAsync(Account account);

    Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId, DateTime? start = null, DateTime? end = null);

    Task AddTransactionAsync(Transaction transaction);

}