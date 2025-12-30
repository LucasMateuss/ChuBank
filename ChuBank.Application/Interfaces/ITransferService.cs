using ChuBank.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Application.Interfaces;

public interface ITransferService
{
    Task PerformTransferAsync(TransferDto dto);
}
