using ECommerceBE.Application.Repositories;
using ECommerceBE.Domain.Entities;
using ECommerceBE.Persistence.Contexts;

namespace ECommerceBE.Persistence.Repositories
{
    public class CustomerWriteRepository : WriteRepository<Customer>, ICustomerWriteRepository
    {
        public CustomerWriteRepository(ECommerceBEDbContext context) : base(context)
        {
        }
    }
}
