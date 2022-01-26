using FluentValidation;
using ImMicro.Contract.Service.User;
using ImMicro.Resources.Service;

namespace ImMicro.Business.User.Validator
{
    public class GetTokenContractServiceRequestValidator: AbstractValidator<GetTokenContractServiceRequest>
    {
        public GetTokenContractServiceRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty().WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "Email"))
                .MaximumLength(320).WithMessage(string.Format(ServiceResponseMessage.PROPERTY_MAX_LENGTH_ERROR, "Email", 320))
                .EmailAddress().WithMessage(string.Format(ServiceResponseMessage.PROPERTY_INVALID, "Email"));

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