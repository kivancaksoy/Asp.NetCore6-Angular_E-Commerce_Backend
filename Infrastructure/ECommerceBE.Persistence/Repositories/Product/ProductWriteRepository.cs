using ECommerceBE.Application.Repositories;
using ECommerceBE.Domain.Entities;
using ECommerceBE.Persistence.Contexts;

namespace ECommerceBE.Persistence.Repositories
{
    public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepository
    {
        public ProductWriteRepository(ECommerceBEDbContext context) : base(context)
        {
        }
    }
}
