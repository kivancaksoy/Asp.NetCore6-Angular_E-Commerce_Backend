using ECommerceBE.Application.Abstraction.Services;
using ECommerceBE.Application.Repositories;
using ECommerceBE.Domain.Entities;
using System.Text.Json;

namespace ECommerceBE.Persistence.Services
{
    //ProductService normalde productla ilgili diğer işlemleri de barındırmalıydı,
    //ancak sonradan oluşturulduğu için sadece QrCode ile igili çalışam mevcut.
    public class ProductService : IProductService
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IQRCodeService _qrCodeService;

        public ProductService(IProductReadRepository productReadRepository, IQRCodeService qrCodeService)
        {
            _productReadRepository = productReadRepository;
            _qrCodeService = qrCodeService;
        }

        public async Task<byte[]> QrCodeToProductAsync(string productId)
        {
            Product product = await _productReadRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            var plainObject = new
            {
                product.Id,
                product.Name,
                product.Price,
                product.Stock,
                product.CreatedDate
            };

            //qrcode içine yukarıdaki objeyi eklemek için seialize yaptık.
            //qrcode'u okurken deserialize yaparak objeyi elde edicez.
            string plainText = JsonSerializer.Serialize(plainObject);

            byte[] qrCodeGraphic = _qrCodeService.GenerateQRCode(plainText);

            return qrCodeGraphic;

        }
    }
}
