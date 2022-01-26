using FluentValidation;
using ImMicro.Contract.Service.User;
using ImMicro.Resources.Service;

namespace ImMicro.Business.User.Validator
{
    public class RefreshTokenContractServiceRequestValidator: AbstractValidator<RefreshTokenContractServiceRequest>
    {
        public RefreshTokenContractServiceRequestValidator()
        {
            RuleFor(request => request.Token)
                .NotEmpty().WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "Token"));
        }
    }
}