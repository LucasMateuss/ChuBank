using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ChuBank.Domain.Entities;

namespace ChuBank.Tests.Domain;

public class AccountTests
{
    [Fact]
    public void Deposit_ShouldIncreaseBalance_WhenAmountIsPositive()
    {
        var account = new Account("123", "Dev", "hash123");
        var initialBalance = account.Balance;
        var depositAmount = 100m;

        account.Deposit(depositAmount);

        Assert.Equal(initialBalance + depositAmount, account.Balance);
    }

    [Fact]
    public void Withdraw_ShouldDecreaseBalance_WhenFundIsSufficient()
    {
        var account = new Account("123", "Dev", "hash123");
        account.Deposit(200m);
        var withdrawAmount = 50m;

        account.Withdraw(withdrawAmount);

        Assert.Equal(150m, account.Balance);
    }

    [Fact]
    public void Withdraw_ShouldThrowException_WhenFundIsInsufficient()
    {
        var account = new Account("12345", "Lucas", "hash123");
        account.Deposit(50m);

        var exception = Assert.Throws<InvalidOperationException>(() => account.Withdraw(100m));
        Assert.Equal("Saldo insuficiente", exception.Message);
    }

    [Fact]
    public void Deposit_ShouldAddTransactionToHistory()
    {
        var account = new Account("12345", "Teste Transaction", "hash123");
        var amount = 100m;
        var senderId = Guid.NewGuid();

        account.Deposit(amount, fromAccountId: senderId);

        Assert.Equal(100, account.Balance);

        Assert.Single(account.Transactions);

        var transaction = account.Transactions[0];
        Assert.Equal(amount, transaction.Amount);
        Assert.Equal(TransactionType.Credit, transaction.Type);
        Assert.Equal(senderId, transaction.FromAccountId); 
        Assert.Equal(account.Id, transaction.ToAccountId); 
    }

    [Fact]
    public void Withdraw_ShouldAddTransactionToHistory()
    {
        var account = new Account("12345", "Teste Transaction", "hash123");
        var receiverId = Guid.NewGuid();

        account.Deposit(200);

        account.Withdraw(50, toAccountId: receiverId);

        Assert.Equal(150, account.Balance);

        Assert.Equal(2, account.Transactions.Count);

        var transaction = account.Transactions[1];
        Assert.Equal(50, transaction.Amount);
        Assert.Equal(TransactionType.Debit, transaction.Type);
        Assert.Equal(account.Id, transaction.FromAccountId);
        Assert.Equal(receiverId, transaction.ToAccountId);  
    }
}