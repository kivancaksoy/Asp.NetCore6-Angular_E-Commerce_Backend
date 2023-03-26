using Microsoft.Extensions.Configuration;

namespace ECommerceBE.Persistence
{
    internal static class Configuration
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                try
                {
                    configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ECommerceBE.API"));
                    configurationManager.AddJsonFile("appsettings.json");
                    return configurationManager.GetConnectionString("PostgreSQL");
                }
                catch
                {
                    configurationManager.AddJsonFile("appsettings.Production.json");
                    return configurationManager.GetConnectionString("PostgreSQL");
                }
            }
        }
    }
}
