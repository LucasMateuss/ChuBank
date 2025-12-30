using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Domain.Interfaces;

public interface IHolidayService
{
    Task<bool> IsBusinessDayAsync(DateTime date);
}