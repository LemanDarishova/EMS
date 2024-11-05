using Ems.BusinessLogic.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Validations;

public class SigninUserDtoValidator : AbstractValidator<SigninUserDto>
{
    public SigninUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email is incorrect format")
            .NotEmpty()
            .WithMessage("Email is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}
