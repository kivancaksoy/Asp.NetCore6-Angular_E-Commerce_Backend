using ECommerceBE.Application.Abstraction.Services;
using QRCoder;

namespace ECommerceBE.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {
        public byte[] GenerateQRCode(string text)
        {
            QRCodeGenerator generator = new();
            QRCodeData data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode qrCode = new PngByteQRCode(data);

            //byte[] byteGraphic = qrCode.GetGraphic(10, new byte[] { 84, 99, 71 }, new byte[] { 240, 240, 240 });
            byte[] byteGraphic = qrCode.GetGraphic(10, new byte[] { 0, 0, 0 }, new byte[] { 240, 240, 240 });

            return byteGraphic;
        }
    }
}
