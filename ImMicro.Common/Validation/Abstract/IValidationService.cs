using System;
using ImMicro.Common.Validation.Concrete;

namespace ImMicro.Common.Validation.Abstract
{
    public interface IValidationService
    {
        ValidationResponse Validate(Type type, dynamic request);
    }
}