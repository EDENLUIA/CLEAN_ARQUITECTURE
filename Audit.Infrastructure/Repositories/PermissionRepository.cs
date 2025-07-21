using Audit.Core;
using Audit.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        protected readonly DbContextClass _dbContext;

        public PermissionRepository(DbContextClass context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Permission>> GetAll()
        {
            return await _dbContext.Set<Permission>()
                                    .AsNoTracking()
                                    .ToListAsync();

        }

        public async Task<Permission> GetById(int id)
        {
            
            return await _dbContext.Set<Permission>()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(p => p.Id == id);
        
            
        }

        public async Task Add(Permission permission)
        {
            await _dbContext.Set<Permission>().AddAsync(permission);
        }

        public async Task Update(Permission permission)
        {
           var result = _dbContext.Set<Permission>().Update(permission).Entity;
        }
    }
}
