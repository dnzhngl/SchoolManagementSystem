using SMS.Core.Data.Repositories;
using SMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Core.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : Entity<int>;

        int SaveChanges();
    }
}
