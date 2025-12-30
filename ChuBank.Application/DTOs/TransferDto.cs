using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Application.DTOs
{
    public record TransferDto(Guid FromAccountId, Guid ToAccountId, decimal Amount);
}
