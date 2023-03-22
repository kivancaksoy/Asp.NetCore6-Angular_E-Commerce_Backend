using MediatR;

namespace ECommerceBE.Application.Features.Queries.AuthorizationEndpoint.GetRolestoEndpoints
{
    public class GetRolestoEndpointQueryRequest : IRequest<GetRolestoEndpointQueryResponse>
    {
        public string Code { get; set; }
        public string Menu { get; set; }
    }
}