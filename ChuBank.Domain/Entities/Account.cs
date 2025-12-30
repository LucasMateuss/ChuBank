using ChuBank.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public string Number { get; private set; } = string.Empty;
    public string Holder { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";
    public List<Transaction> Transactions { get; private set; } = new();

    protected Account() { }

    public Account(string number, string holder, string passwordHash)
    {
        Id = Guid.NewGuid();
        Number = number;
        Holder = holder;
        PasswordHash = passwordHash;
        Balance = 0;
    }

    public void Deposit(decimal amount, Guid? fromAccountId = null)
    {
        if (amount <= 0)
        {
            throw new ChuBankException("Valor deve ser maior que zero");
        }

        Balance += amount;

        Transactions.Add(new Transaction(
            accountId: Id,
            amount: amount,
            type: TransactionType.Credit,
            fromAccountId: fromAccountId,
            toAccountId: Id 
        ));
    }

    public void Withdraw(decimal amount, Guid? toAccountId = null)
    {
        if (amount <= 0) 
        {
            throw new ChuBankException("Valor deve ser maior que zero");
        }

        if (Balance < amount)
        {
            throw new ChuBankException("Saldo insuficiente");

        }

        Balance -= amount;

        Transactions.Add(new Transaction(
            accountId: Id,
            amount: amount,
            type: TransactionType.Debit,
            fromAccountId: Id, 
            toAccountId: toAccountId
        ));
    }
}