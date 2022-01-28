using System.Threading.Tasks;
using ImMicro.Business.User.Abstract;
using ImMicro.Common.Auth.Concrete;
using ImMicro.Common.BaseModels.Api;
using ImMicro.Contract.App.User;
using ImMicro.Contract.Service.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImMicro.Api.Controllers.V1
{
    /// <summary>
    /// Login Controller
    /// </summary>
    [ApiVersion("1.0")]
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// The constructor of <see cref="LoginController"/>.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public LoginController(IUserService userService) => _userService = userService;

        /// <summary>
        /// Get Token
        /// </summary>
        /// <returns>Returns user jwt token.</returns>
        [AllowAnonymous]
        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccessTokenContract))]
        public async Task<ActionResult> Token([FromBody] GetTokenContract tokenContract)
        {
            var result = await _userService.GetTokenAsync(Mapper.Map<GetTokenContractServiceRequest>(tokenContract));
            return ApiResponse.CreateResult(result);
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <returns>Returns user jwt refresh token.</returns>
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccessTokenContract))]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenContract tokenContract)
        {
            var result = await _userService.RefreshTokenAsync(Mapper.Map<RefreshTokenContractServiceRequest>(tokenContract));
            return ApiResponse.CreateResult(result);
        }
    }
}
