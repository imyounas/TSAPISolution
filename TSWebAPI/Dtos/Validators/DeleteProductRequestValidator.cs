using FluentValidation;

namespace TSWebAPI.Dtos.Validators
{
    public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequestDto>
    {
        public DeleteProductRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id cannot be empty.");
                
        }
    }
}
