using ECommerceBE.Application.DTOs.User;

namespace ECommerceBE.Application.Abstraction.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
    }
}
