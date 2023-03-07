using ECommerceBE.Application.Abstraction.Services;
using ECommerceBE.Application.DTOs.User;
using MediatR;

namespace ECommerceBE.Application.Features.Commands.AppUser.CreateUSer
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            CreateUserResponse response = await _userService.CreateAsync(new()
            {
                Email = request.Email,
                NameSurname = request.NameSurname,
                Username = request.Username,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
            });

            return new()
            {
                Message= response.Message,
                Succeeded= response.Succeeded,
            };
        }
    }
}
