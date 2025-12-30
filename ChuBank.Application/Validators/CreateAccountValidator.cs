using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ChuBank.Application.DTOs;

namespace ChuBank.Application.Validators;

public class CreateAccountValidator : AbstractValidator<CreateAccountDto>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.Holder)
            .NotEmpty().WithMessage("O nome do titular é obrigatório.")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres.");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("O número da conta é obrigatório.")
            .Matches(@"^\d+$").WithMessage("A conta deve conter apenas números.");
    }
}