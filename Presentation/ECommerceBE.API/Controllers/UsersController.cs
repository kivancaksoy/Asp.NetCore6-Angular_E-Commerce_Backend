using ECommerceBE.Application.Abstraction.Services;
using ECommerceBE.Application.Features.Commands.AppUser.CreateUSer;
using MediatR;
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

        //mail göndeirmi testi için oluşturuldu.
        [HttpGet]
        public async Task<IActionResult> ExampleMailTest()
        {
            await _mailService.SendMessageAsync("pythonders3@gmail.com", "Örnek Mail", "<strong>Bu bir örnek maildir.</strong>");
            return Ok();
        }

    }
}
