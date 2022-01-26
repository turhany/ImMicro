using System;
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

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <returns>Returns if user exist on database will be send to email for forgot password.</returns>
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataResponse))]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _userService.CreateForgotPasswordTokenAsync(Mapper.Map<ForgotPasswordServiceRequest>(request));
            return ApiResponse.CreateResult(result);
        }

        /// <summary>
        /// Reset the password with the token if still is valid and not used.
        /// </summary>
        /// <returns>Returns if user reset password token is not used.</returns>
        [AllowAnonymous]
        [HttpGet("reset-password/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataResponse))]
        public async Task<ActionResult> ResetPassword(Guid id)
        {
            var result = await _userService.GetForgotPasswordTokenAsync(id);
            return ApiResponse.CreateResult(result);
        }

        /// <summary>
        /// Reset the password using the token, new password, and new password confirmation.The token is in the mail link.
        /// </summary>
        /// <param name="id">The forgot password token.</param>
        /// <param name="request">The request <see cref="ResetPasswordRequest"/> model.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("reset-password/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataResponse))]
        public async Task<ActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordRequest request)
        {
            var result = await _userService.ResetPasswordAsync(id, Mapper.Map<ResetPasswordServiceRequest>(request));
            return ApiResponse.CreateResult(result);
        }

        /// <summary>
        /// Change password with using old password, new password and new password confirm.
        /// </summary>
        /// <param name="id">The user identifier.</param>
        /// <param name="request">The request <see cref="ChangePasswordRequest"/> model.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("change-password/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataResponse))]
        public async Task<ActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
        {
            var result = await _userService.ChangePasswordAsync(id, Mapper.Map<ChangePasswordServiceRequest>(request));
            return ApiResponse.CreateResult(result);
        }

        /// <summary>
        /// Confirm email.The confirm token is in the mail link.
        /// </summary>
        /// <param name="id">The confirm email token.</param>
        /// <returns>JWT Token for login.</returns>
        [AllowAnonymous]
        [HttpGet("confirm-email/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccessTokenContract))]
        public async Task<ActionResult> ConfirmEmail(Guid id)
        {
            var result = await _userService.ConfirmEmailAsync(id);
            return ApiResponse.CreateResult(result);
        }

        /// <summary>
        /// Resend confirm email.When user not access the account confirm mail he can be request a new confirm mail.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("resend-confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataResponse))]
        public async Task<ActionResult> ResendConfirmEmail([FromBody] ResendConfirmEmailRequest request)
        {
            var result = await _userService.ResendConfirmEmailAsync(Mapper.Map<ResendConfirmEmailServiceRequest>(request));
            return ApiResponse.CreateResult(result);
        }
    }
}
