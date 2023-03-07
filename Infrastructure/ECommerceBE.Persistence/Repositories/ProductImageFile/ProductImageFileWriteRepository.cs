using ECommerceBE.Application.Repositories;
using ECommerceBE.Domain.Entities;
using ECommerceBE.Persistence.Contexts;

namespace ECommerceBE.Persistence.Repositories
{
    public class ProductImageFileWriteRepository : WriteRepository<ProductImageFile>, IProductImageFileWriteRepository
    {
        public ProductImageFileWriteRepository(ECommerceBEDbContext context) : base(context)
        {
        }
    }
}
