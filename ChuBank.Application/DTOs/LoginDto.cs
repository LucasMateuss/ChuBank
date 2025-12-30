using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Application.DTOs;

public record LoginDto(string AccountNumber, string Password);
