using ECommerceBE.Application.Features.Commands.AuthorizationEndpoint.AssignRoleEndpoint;
using ECommerceBE.Application.Features.Queries.AuthorizationEndpoint.GetRolestoEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleEndpoint(AssignRoleEndpointCommandRequest assignRoleEndpointCommandRequest)
        {
            assignRoleEndpointCommandRequest.Type = typeof(Program);
            AssignRoleEndpointCommandResponse response = await _mediator.Send(assignRoleEndpointCommandRequest);
            return Ok(response);
        }

        [HttpPost("get-roles-to-endpoint")]
        public async Task<IActionResult> GetRolestoEndpoint(GetRolestoEndpointQueryRequest getRolestoEndpointQueryRequest)
        {
            GetRolestoEndpointQueryResponse response = await _mediator.Send(getRolestoEndpointQueryRequest);

            return Ok(response);
        }
    }
}
