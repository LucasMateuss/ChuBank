using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Application.DTOs
{
    public record AccountResponseDto(Guid Id, string Number, string Holder, decimal Balance);
}
