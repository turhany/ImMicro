using ImMicro.Validation.Models;
namespace ImMicro.Validation.Abstract
{
    public interface IValidationService
    {
        ValidationResponse Validate(Type type, dynamic request);
    }
}