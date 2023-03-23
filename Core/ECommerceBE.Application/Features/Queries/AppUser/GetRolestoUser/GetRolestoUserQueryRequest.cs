using MediatR;

namespace ECommerceBE.Application.Features.Queries.AppUser.GetRolestoUser
{
    public class GetRolestoUserQueryRequest : IRequest<GetRolestoUserQueryResponse>
    {
        public string UserId { get; set; }
    }
}