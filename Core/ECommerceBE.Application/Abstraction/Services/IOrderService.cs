﻿using ECommerceBE.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceBE.Application.Abstraction.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrder createOrder);
    }
}
