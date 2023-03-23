using ECommerceBE.Application.Abstraction.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBE.Application.Features.Queries.AppUser.GetRolestoUser
{
    public class GetRolestoUserQueryHandler : IRequestHandler<GetRolestoUserQueryRequest, GetRolestoUserQueryResponse>
    {
        readonly IUserService _userService;

        public GetRolestoUserQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetRolestoUserQueryResponse> Handle(GetRolestoUserQueryRequest request, CancellationToken cancellationToken)
        {
            var userRoles = await _userService.GetRolesToUserAsync(request.UserId);
            return new()
            {
                UserRoles = userRoles,
            };
        }
    }
}
