﻿using ECommerceBE.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBE.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
