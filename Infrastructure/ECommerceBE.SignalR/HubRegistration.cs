﻿using ECommerceBE.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;

namespace ECommerceBE.SignalR
{
    public static class HubRegistration
    {
        public static void MapHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<ProductHub>("/products-hub");
        }
    }
}
