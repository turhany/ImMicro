using FluentValidation;
using ImMicro.Contract.Service.User;
using ImMicro.Resources.Service;

namespace ImMicro.Business.User.Validator
{
    public class ResetPasswordServiceRequestValidator : AbstractValidator<ResetPasswordServiceRequest>
    {
        public ResetPasswordServiceRequestValidator()
        {
            RuleFor(request => request.Password)
                .NotEmpty()
                .WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "Password"))
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain one or more capital letters.")
                .Matches("[a-z]").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
                .Matches(@"\d").WithMessage("'{PropertyName}' must contain one or more digits.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("'{ PropertyName}' must contain one or more special characters.");
        }
    }
}
