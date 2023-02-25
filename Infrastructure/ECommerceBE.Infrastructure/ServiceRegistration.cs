
using ECommerceBE.Application.Abstraction.Storage;
using ECommerceBE.Infrastructure.Enums;
using ECommerceBE.Infrastructure.Services.Storage;
using ECommerceBE.Infrastructure.Services.Storage.Azure;
using ECommerceBE.Infrastructure.Services.Storage.Local;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBE.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
        }

        //genelikle enum yerine bu şekilde kullanım tercih edilir. Daha temiz bir codedur.
        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }

        //aşağıdaki gibi enum ile kullanım doğru değel çünkü sürekli koda bağımlı bir değişim oluyor. genellikleri yularıdaki gibi kullanım tercih edilir.
        public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:

                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
