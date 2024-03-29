﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Filtery.Models;
using ImMicro.Business.User.Abstract;
using ImMicro.Common.BaseModels.Api;
using ImMicro.Contract.App.User;
using ImMicro.Contract.Service.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImMicro.Api.Controllers.V1
{
    /// <summary>
    /// Users Controller
    /// </summary>
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// User Controller
        /// </summary>
        /// <param name="userService"></param>
        public UsersController(IUserService userService) => _userService = userService;

        /// <summary>
        /// Get User
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Root")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserView))]
        public async Task<ActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var result = await _userService.GetAsync(id, cancellationToken);
            return ApiResponse.CreateResult(result);
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Root")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            if (request == null) return ApiResponse.InvalidInputResult;

            var result = await _userService.CreateAsync(Mapper.Map<CreateUserRequestServiceRequest>(request), cancellationToken);
            return ApiResponse.CreateResult(result);
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Root")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUserRequest request, Guid id, CancellationToken cancellationToken)
        {
            if (request == null) return ApiResponse.InvalidInputResult;
            var model = Mapper.Map<UpdateUserRequestServiceRequest>(request);
            model.Id = id;

            var result = await _userService.UpdateAsync(model, cancellationToken);
            return ApiResponse.CreateResult(result);
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Root")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return ApiResponse.InvalidInputResult;

            var result = await _userService.DeleteAsync(id, cancellationToken);
            return ApiResponse.CreateResult(result);
        }

        /// <summary>
        /// User Search
        /// </summary>
        /// <returns></returns>
        [HttpPost("search")]
        [Authorize(Roles = "Root")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Search([FromBody] FilteryRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.SearchAsync(request, cancellationToken);
            return ApiResponse.CreateResult(result);
        }
    }
}