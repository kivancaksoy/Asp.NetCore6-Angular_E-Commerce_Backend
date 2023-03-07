using ECommerceBE.Application.Repositories;
using ECommerceBE.Persistence.Contexts;

namespace ECommerceBE.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(ECommerceBEDbContext context) : base(context)
        {
        }
    }
}
