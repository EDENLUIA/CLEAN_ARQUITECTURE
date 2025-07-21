using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Core.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAll();
        Task<Permission> GetById(int id);
        Task Add(Permission permission);
        Task Update(Permission permission);

    }
}
