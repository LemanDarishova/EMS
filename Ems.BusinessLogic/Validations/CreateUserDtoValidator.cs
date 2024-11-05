using Ems.BusinessLogic.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Validations;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email is incorrect format")
            .NotEmpty()
            .WithMessage("Email is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long");
    }
}
