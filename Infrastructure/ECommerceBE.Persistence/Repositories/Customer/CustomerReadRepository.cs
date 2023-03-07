using ECommerceBE.Application.Repositories;
using ECommerceBE.Domain.Entities;
using ECommerceBE.Persistence.Contexts;

namespace ECommerceBE.Persistence.Repositories
{
    public class CustomerReadRepository : ReadRepository<Customer>, ICustomerReadRepository
    {
        public CustomerReadRepository(ECommerceBEDbContext context) : base(context)
        {
        }
    }
}
