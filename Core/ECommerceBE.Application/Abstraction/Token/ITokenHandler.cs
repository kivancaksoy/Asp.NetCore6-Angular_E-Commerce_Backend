namespace ECommerceBE.Application.Abstraction.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(int second);
        string CreateRefreshToken();
    }
}
