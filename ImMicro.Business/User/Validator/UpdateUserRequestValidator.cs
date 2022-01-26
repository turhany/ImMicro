using FluentValidation;
using FluentValidation.Validators;
using ImMicro.Contract.Service.User;
using ImMicro.Resources.Service;

#pragma warning disable 618

namespace ImMicro.Business.User.Validator
{
    public class UpdateUserRequestValidator: AbstractValidator<UpdateUserRequestServiceRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(request => request.FirstName)
                .NotEmpty().WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "First Name"))
                .MaximumLength(40).WithMessage(string.Format(ServiceResponseMessage.PROPERTY_MAX_LENGTH_ERROR, "First Name", 40));

            RuleFor(request => request.LastName)
                .NotEmpty().WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "Last Name"))
                .MaximumLength(40).WithMessage(string.Format(ServiceResponseMessage.PROPERTY_MAX_LENGTH_ERROR, "Last Name", 40));

            RuleFor(request => request.Email)
                .NotEmpty().WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "Email"))
                .MaximumLength(320).WithMessage(string.Format(ServiceResponseMessage.PROPERTY_MAX_LENGTH_ERROR, "Email", 320))
                .EmailAddress(EmailValidationMode.Net4xRegex).WithMessage(string.Format(ServiceResponseMessage.PROPERTY_INVALID, "Email"));

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