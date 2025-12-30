using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuBank.Domain.Entities;

namespace ChuBank.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(Account account);
}
