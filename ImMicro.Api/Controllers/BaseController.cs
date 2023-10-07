using AutoMapper;
using ImMicro.Common.Application;
using ImMicro.Common.Constans;
using Microsoft.AspNetCore.Mvc;

namespace ImMicro.Api.Controllers
{
    /// <summary>
    /// Api base controller
    /// </summary>
    [ApiController] 
    [Produces(AppConstants.JsonContentType)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseController : Controller
    {
        /// <summary>
        /// Auto Mapper
        /// </summary>
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Base Controller
        /// </summary>
        public BaseController()
        {
            var services = ApplicationContext.Context.HttpContext.RequestServices;
            Mapper = (IMapper)services.GetService(typeof(IMapper));              
        }
    }
}