using ECommerceBE.Application.Abstraction.Services.Authentications;

namespace ECommerceBE.Application.Abstraction.Services
{
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {
    }
}
