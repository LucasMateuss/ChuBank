using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Domain.Exceptions;

public class ChuBankException : Exception
{
    public ChuBankException(string message) : base(message)
    { }
}
