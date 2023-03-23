using MediatR;

namespace ECommerceBE.Application.Features.Queries.AppUser.GetAllUsers
{
    public class GetAllUsersRequest : IRequest<GetAllUsersResponse>
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }
}