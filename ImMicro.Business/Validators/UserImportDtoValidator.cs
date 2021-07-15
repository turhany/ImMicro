using FluentValidation;
using ImMicro.Contract.Dtos;

namespace ImMicro.Business.Validators
{
    public class UserImportDtoValidator : AbstractValidator<UserImportDto>
    {
        public UserImportDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull();
            
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull();
            
            RuleFor(x => x.Surname)
                .NotEmpty()
                .NotNull();
        }
    }
}