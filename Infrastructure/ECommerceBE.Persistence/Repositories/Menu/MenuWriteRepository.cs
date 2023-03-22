using ECommerceBE.Application.Repositories;
using ECommerceBE.Domain.Entities;
using ECommerceBE.Persistence.Contexts;

namespace ECommerceBE.Persistence.Repositories
{
    public class MenuWriteRepository : WriteRepository<Menu>, IMenuWriteepository
    {
        public MenuWriteRepository(ECommerceBEDbContext context) : base(context)
        {
        }
    }
}
