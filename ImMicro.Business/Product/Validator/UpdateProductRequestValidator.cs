using FluentValidation;
using ImMicro.Contract.Service.Product;
using ImMicro.Resources.Service;

namespace ImMicro.Business.Product.Validator;

public class UpdateProductRequestValidator: AbstractValidator<UpdateProductRequestServiceRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "Title"))
            .MaximumLength(200)
            .WithMessage(string.Format(ServiceResponseMessage.PROPERTY_MAX_LENGTH_ERROR, "Title", 200));

        RuleFor(request => request.Description)
            .NotEmpty().WithMessage(string.Format(ServiceResponseMessage.PROPERTY_REQUIRED, "Description"))
            .MaximumLength(500)
            .WithMessage(string.Format(ServiceResponseMessage.PROPERTY_MAX_LENGTH_ERROR, "Description", 500));
    }
}