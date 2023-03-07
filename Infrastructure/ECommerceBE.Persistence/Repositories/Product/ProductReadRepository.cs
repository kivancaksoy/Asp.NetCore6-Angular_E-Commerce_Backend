using ECommerceBE.Application.Repositories;
using ECommerceBE.Domain.Entities;
using ECommerceBE.Persistence.Contexts;

namespace ECommerceBE.Persistence.Repositories
{
    public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
    {
        public ProductReadRepository(ECommerceBEDbContext context) : base(context)
        {
        }
    }
}
