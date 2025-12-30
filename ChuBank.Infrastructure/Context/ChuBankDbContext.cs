using ChuBank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ChuBank.Infrastructure.Context;

public class ChuBankDbContext : DbContext
{
    public ChuBankDbContext(DbContextOptions<ChuBankDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(e =>
        {
            e.HasKey(a => a.Id);

            e.Property(a => a.Number)
                .IsRequired()
                .HasMaxLength(20);

            e.HasIndex(a => a.Number).IsUnique();

            e.Property(a => a.Holder)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(a => a.Balance)
                .HasPrecision(18, 2);

            e.HasMany(a => a.Transactions)
                .WithOne()
                .HasForeignKey(t => t.AccountId);
        });

        modelBuilder.Entity<Transaction>(e =>
        {
            e.HasKey(t => t.Id);

            e.Property(t => t.Id)
                .ValueGeneratedNever();

            e.Property(t => t.Amount)
                .HasPrecision(18, 2);

            e.Property(t => t.Type)
                .HasConversion<string>(); 
        });
    }
}