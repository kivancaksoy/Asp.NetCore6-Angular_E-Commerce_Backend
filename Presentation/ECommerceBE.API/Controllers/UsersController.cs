using ECommerceBE.Application.Abstraction.Services;
using ECommerceBE.Application.CustomAttributes;
using ECommerceBE.Application.Enums;
using ECommerceBE.Application.Features.Commands.AppUser.AssignRoleToUser;
using ECommerceBE.Application.Features.Commands.AppUser.CreateUSer;
using ECommerceBE.Application.Features.Commands.AppUser.UpdatePassword;
using ECommerceBE.Application.Features.Queries.AppUser.GetAllUsers;
using ECommerceBE.Application.Features.Queries.AppUser.GetRolestoUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IMailService _mailService;

        public UsersController(IMediator mediator, IMailService mailService)
        {
            _mediator = mediator;
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            UpdatePasswordCommandResponse response = await _mediator.Send(updatePasswordCommandRequest);
            return Ok();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get All Users", Menu = "Users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersRequest getAllUsersRequest)
        {
            GetAllUsersResponse response = await _mediator.Send(getAllUsersRequest);
            return Ok(response);
        }

        [HttpGet("get-roles-to-user/{UserId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles To User", Menu = "Users")]
        public async Task<IActionResult> GetRolestoUser([FromRoute] GetRolestoUserQueryRequest getRolestoUserQueryRequest)
        {
            GetRolestoUserQueryResponse response = await _mediator.Send(getRolestoUserQueryRequest);
            return Ok(response);
        }

        [HttpPost("assign-role-to-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Assign Role To Users", Menu = "Users")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserCommandRequest assignRoleToUserCommandRequest)
        {
            AssignRoleToUserCommandResponse response = await _mediator.Send(assignRoleToUserCommandRequest);
            return Ok(response);
        }

    }
}
