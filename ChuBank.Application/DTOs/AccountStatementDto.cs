using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Application.DTOs;

public class AccountStatementDto
{
    public string AccountNumber { get; set; } = string.Empty;
    public string Holder { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public List<TransactionItemDto> Transactions { get; set; } = new();
}

public class TransactionItemDto
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;       
    public string Description { get; set; } = string.Empty;
}
