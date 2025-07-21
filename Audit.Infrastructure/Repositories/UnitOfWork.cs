using Audit.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContextClass _dbContext;
        public IPermissionRepository Permissions { get; }


        public UnitOfWork(DbContextClass dbContext,
                           IPermissionRepository permissionRepository)
        {
            _dbContext = dbContext;
            Permissions = permissionRepository;
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}
