using FluentValidation;
using FluentValidation.Validators;
using ImMicro.Contract.Service.User;
using ImMicro.Resources.Service;

#pragma warning disable 618

namespace ImMicro.Business.User.Validator
{
    public class ForgotPasswordServiceRequestValidator : AbstractValidator<ForgotPasswordServiceRequest>
    {
        public ForgotPasswordServiceRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty().WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "Email"))
                .MaximumLength(320).WithMessage(string.Format(ServiceResponseMessage.PROPERTY_MAX_LENGTH_ERROR, "Email", 320))
                .EmailAddress(EmailValidationMode.Net4xRegex)
                .WithMessage(string.Format(ServiceResponseMessage.PROPERTY_INVALID, "Email"));
        }
    }
}
