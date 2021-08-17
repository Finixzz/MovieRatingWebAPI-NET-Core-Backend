using MediatR;
using Microsoft.AspNetCore.Identity;
using MovieRatingEngine.AhmetDurmic.WebApi.DTOS;
using MovieRatingEngine.DAL.Utils.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRatingEngine.AhmetDurmic.WebApi.Services.Commands
{
    public class CreateUserCommand : IRequest<object>
    {
        public RegisterUserDTO RegisterUserDTO { get; set; }

        public CreateUserCommand(RegisterUserDTO RegisterUserDTO)
        {
            this.RegisterUserDTO = RegisterUserDTO;
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, object>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public CreateUserCommandHandler(UserManager<IdentityUser> _userManager)
        {
            this._userManager = _userManager;
        }


        public async Task<object> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            IdentityUser newUser = new IdentityUser()
            {
                Email = request.RegisterUserDTO.Email,
                UserName = request.RegisterUserDTO.Email

            };

            var createdUserResult = await _userManager.CreateAsync(newUser, request.RegisterUserDTO.Password);

            if (!createdUserResult.Succeeded)
                return createdUserResult;

            var assignUserToRolesResult = request.RegisterUserDTO.Roles == null ? await _userManager.AddToRoleAsync(newUser, RoleConstants.USER_ROLE) : await _userManager.AddToRolesAsync(newUser, request.RegisterUserDTO.Roles);

            if (!assignUserToRolesResult.Succeeded)
                return assignUserToRolesResult;

            return createdUserResult;
        }
    }
}
