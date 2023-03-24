namespace ECommerceBE.Application.Abstraction.Services
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string text);
    }
}
