using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Core.Interfaces
{
    public interface IPermissionElasticsearchRepository
    {
        Task<bool> IndexAsync(Permission permission);
    }
}
