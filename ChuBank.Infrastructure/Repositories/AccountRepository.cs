using Microsoft.EntityFrameworkCore;
using ChuBank.Domain.Entities;
using ChuBank.Domain.Interfaces;
using ChuBank.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ChuBankDbContext _context;

    public AccountRepository(ChuBankDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _context.Accounts
            .Include(a => a.Transactions) 
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByNumberAsync(string number)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.Number == number);
    }

    public async Task UpdateAsync(Account account)
    {
        if (_context.Entry(account).State == EntityState.Detached)
        {
            _context.Accounts.Attach(account);
            _context.Entry(account).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId, DateTime? start = null, DateTime? end = null)
    {
        var query = _context.Transactions
            .Where(t => t.AccountId == accountId);

        if (start.HasValue)
        {
            query = query.Where(t => t.TransactionDate >= start.Value);
        }

        if (end.HasValue)
        {
            query = query.Where(t => t.TransactionDate <= end.Value);
        }

        return await query
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }
}