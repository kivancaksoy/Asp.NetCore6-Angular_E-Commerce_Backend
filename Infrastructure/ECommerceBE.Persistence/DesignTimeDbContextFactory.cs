using ECommerceBE.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECommerceBE.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ECommerceBEDbContext>
    {
        public ECommerceBEDbContext CreateDbContext(string[] args)
        {

            DbContextOptionsBuilder<ECommerceBEDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);
            return new ECommerceBEDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
