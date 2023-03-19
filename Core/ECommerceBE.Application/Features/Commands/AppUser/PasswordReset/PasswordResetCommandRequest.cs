﻿using MediatR;

namespace ECommerceBE.Application.Features.Commands.AppUser.PasswordReset
{
    public class PasswordResetCommandRequest : IRequest<PasswordResetCommandResponse>
    {
        public string Email { get; set; }
    }
}