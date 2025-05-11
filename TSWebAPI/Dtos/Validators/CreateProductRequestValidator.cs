using FluentValidation;

namespace TSWebAPI.Dtos.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequestDto>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .MaximumLength(500)
                .WithMessage("Name must not exceed 500 characters.");
            RuleFor(x => x.Data)
                .Must(data => data == null || data.Count > 0)
                .WithMessage("Data must be provided if present.");
        }
    }
}
