using ECommerceBE.Domain.Entities.Identity;

namespace ECommerceBE.Application.Abstraction.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(int second, AppUser user);
        string CreateRefreshToken();
    }
}
