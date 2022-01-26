using FluentValidation;
using ImMicro.Contract.Service.User;
using ImMicro.Resources.Service;

namespace ImMicro.Business.User.Validator
{
    public class ChangePasswordServiceRequestValidator : AbstractValidator<ChangePasswordServiceRequest>
    {
        public ChangePasswordServiceRequestValidator()
        {
            RuleFor(request => request.NewPassword)
                .NotEmpty()
                .WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "NewPassword"))
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain one or more capital letters.")
                .Matches("[a-z]").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
                .Matches(@"\d").WithMessage("'{PropertyName}' must contain one or more digits.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("'{ PropertyName}' must contain one or more special characters.");

            RuleFor(request => request.NewPasswordConfirm)
                .NotEmpty()
                .WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "NewPasswordConfirm"))
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain one or more capital letters.")
                .Matches("[a-z]").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
                .Matches(@"\d").WithMessage("'{PropertyName}' must contain one or more digits.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("'{ PropertyName}' must contain one or more special characters.");

            RuleFor(request => request.NewPassword)
                .Equal(request => request.NewPasswordConfirm)
                .WithMessage("New password must be equal password confirm.");

            RuleFor(request => request.OldPassword)
                .NotEmpty()
                .WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "OldPassword"))
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain one or more capital letters.")
                .Matches("[a-z]").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
                .Matches(@"\d").WithMessage("'{PropertyName}' must contain one or more digits.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("'{ PropertyName}' must contain one or more special characters.");

            RuleFor(request => request.NewPassword)
                .NotEqual(request => request.OldPassword)
                .WithMessage("New password will be different old one.");
        }
    }
}
