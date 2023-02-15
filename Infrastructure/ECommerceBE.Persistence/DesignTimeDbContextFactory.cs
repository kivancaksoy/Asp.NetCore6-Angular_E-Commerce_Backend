using ECommerceBE.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
