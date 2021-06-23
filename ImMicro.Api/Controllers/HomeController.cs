using Microsoft.AspNetCore.Mvc; 

namespace ImMicro.Api.Controllers
{
    /// <summary>
    /// Api Home controller
    /// </summary>
    [Route("")]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Home Controller
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [HttpGet("Index")]
        public RedirectResult Home()
        {
            return Redirect($"{Request.Scheme}://{Request.Host.ToUriComponent()}/swagger");
        }
    }
}