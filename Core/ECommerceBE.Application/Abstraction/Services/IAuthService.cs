using ECommerceBE.Application.Abstraction.Services.Authentications;

namespace ECommerceBE.Application.Abstraction.Services
{
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {
        Task PasswordResetAsync(string email);
        Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
    }
}
