using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ChuBank.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public Guid? FromAccountId { get; private set; } 
    public Guid? ToAccountId { get; private set; }

    protected Transaction() { }

    public Transaction(Guid accountId, decimal amount, TransactionType type, Guid? fromAccountId = null, Guid? toAccountId = null)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Amount = amount;
        Type = type;
        TransactionDate = DateTime.UtcNow;
        FromAccountId = fromAccountId;
        ToAccountId = toAccountId;
    }
}

public enum TransactionType
{
    Credit,
    Debit
}