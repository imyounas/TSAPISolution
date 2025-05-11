using FluentValidation;

namespace TSWebAPI.Dtos.Validators
{
    public class ProductFilterRequestValidator : AbstractValidator<ProductFilterRequestDto>
    {
        public ProductFilterRequestValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(500)
                .WithMessage("Name must not exceed 500 characters.");

            RuleFor(x => x.PageNo)
                .GreaterThan(0)
                .WithMessage("PageNo must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.");
        }
    }
}
