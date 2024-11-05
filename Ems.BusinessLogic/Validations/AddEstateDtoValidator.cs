using Ems.BusinessLogic.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Validations;
public class AddEstateDtoValidator : AbstractValidator<AddEstateDto>
{
    public AddEstateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MinimumLength(5)
            .WithMessage("Title must be greater than 5 characters");

        RuleFor(x => x.Area)
            .NotEmpty()
            .WithMessage("Area is required")
            .InclusiveBetween(100, 1000000)
            .WithMessage("Area must be between 100 and 1000000 square meters");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MinimumLength(10)
            .WithMessage("Address must be at least 10 characters long");

        RuleFor(x => x.Price)
            .NotNull()
            .WithMessage("Price is required")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Price must be a positive value")
            .LessThanOrEqualTo(1000)
            .WithMessage("Price must be less than or equal to 1000");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category is required");
    }
}
