using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using ImMicro.Business.Services.Abstract;
using ImMicro.Contract.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImMicro.Api.Controllers
{
    /// <inheritdoc />
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        /// <inheritdoc />
        public UserController( IUserService userService)
        {
            _userService = userService;
        }
        
        /// <summary>
        /// Import user json file
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Init([CustomizeValidator(RuleSet = "importFile")] [FromForm]FileContract file, CancellationToken cancellationToken)
        {
            await using (var memoryStream = new MemoryStream())
            {
                await file.FileData.CopyToAsync(memoryStream, cancellationToken);
                await _userService.ImportDataAsync(memoryStream, cancellationToken);
            }

            return Ok();
        }
        
    }
}