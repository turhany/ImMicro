using System; 
using FluentValidation;
using ImMicro.Common.Resources;

namespace ImMicro.Business.Validators
{
    public class UrlValidator : AbstractValidator<string>
    {
        public UrlValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .NotNull()
                .Must(IsUri).WithMessage(Literals.UrlValidateError_Message);
        }

        private bool IsUri(string link)
        {
            var response = false;
            if (Uri.TryCreate(link, UriKind.RelativeOrAbsolute,out Uri parsedUri))
            {
                try
                {
                    var query = parsedUri.Query;
                    response = true;
                }
                catch 
                { 
                    //Ignores
                }
                return response;
            }

            return true;
        }
    }
}