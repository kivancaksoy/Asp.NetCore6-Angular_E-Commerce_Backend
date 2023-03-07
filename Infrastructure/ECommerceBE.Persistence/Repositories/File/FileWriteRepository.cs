using ECommerceBE.Application.Repositories;
using ECommerceBE.Persistence.Contexts;

namespace ECommerceBE.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(ECommerceBEDbContext context) : base(context)
        {
        }
    }
}
