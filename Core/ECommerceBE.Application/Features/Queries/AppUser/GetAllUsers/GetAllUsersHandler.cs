using ECommerceBE.Application.Abstraction.Services;
using MediatR;

namespace ECommerceBE.Application.Features.Queries.AppUser.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, GetAllUsersResponse>
    {
        readonly IUserService _userService;

        public GetAllUsersHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetAllUsersResponse> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
        {
            var datas = await _userService.GetAllUsersAsync(request.Page, request.Size);

            return new()
            {
                Users = datas,
                TotalUsersCount = _userService.TotalUsersCount
            };
        }
    }
}
