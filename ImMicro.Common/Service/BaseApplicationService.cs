using AutoMapper;
using ImMicro.Common.Application;
using ImMicro.Common.Validation.Abstract;

namespace ImMicro.Common.Service
{
    public class BaseApplicationService
    {
        public IMapper Mapper { get; set; }
        public IValidationService ValidationService { get; set; }

        public BaseApplicationService()
        {
            var services = ApplicationContext.Context?.HttpContext?.RequestServices ?? ApplicationContext.WorkerServiceProvider;
            ValidationService = (IValidationService)services.GetService(typeof(IValidationService));
            Mapper = (IMapper)services.GetService(typeof(IMapper));
        }
    }
}