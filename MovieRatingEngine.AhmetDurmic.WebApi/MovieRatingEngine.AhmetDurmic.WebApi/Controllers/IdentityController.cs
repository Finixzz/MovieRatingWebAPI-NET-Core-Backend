using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieRatingEngine.AhmetDurmic.WebApi.DTOS;
using MovieRatingEngine.AhmetDurmic.WebApi.Services.Commands;
using MovieRatingEngine.AhmetDurmic.WebApi.Services.Queries;
using MovieRatingEngine.AhmetDurmic.WebApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingEngine.AhmetDurmic.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _broker;

        public IdentityController(IMediator _broker)
        {
            this._broker = _broker;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            var createUserCommand = new CreateUserCommand(registerUserDTO);

            IdentityResult createUserCommandResult = (IdentityResult)await _broker.Send(createUserCommand);

            if (!createUserCommandResult.Succeeded)
                return BadRequest(createUserCommandResult);

            return Ok(ResponseConstants.USER_CREATED_SUCCESSFULLY);
        }



        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUserAsync(AuthenticateUserDTO authenticateUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var generateUserJWTQuery = new GenerateUserJWTQuery(authenticateUserDTO);

            var token = (string)await _broker.Send(generateUserJWTQuery);

            if (token == null)
                return BadRequest();

            return Ok(token);
        }

    }
}
