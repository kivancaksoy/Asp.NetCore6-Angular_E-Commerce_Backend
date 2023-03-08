using ECommerceBE.Application.DTOs.User;
using ECommerceBE.Domain.Entities.Identity;

namespace ECommerceBE.Application.Abstraction.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
    }
}
