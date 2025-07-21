using Audit.Core.Interfaces;
using Audit.Core;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Infrastructure.Repositories
{
    public class PermissionElasticsearchRepository : IPermissionElasticsearchRepository
    {
        private readonly IElasticClient _elasticClient;

        public PermissionElasticsearchRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<bool> IndexAsync(Permission permission)
        {
            var response = await _elasticClient.UpdateAsync<Permission>(
                                                permission.Id.ToString(),            
                                                 u => u.Index("permissions")          
                                                .Doc(permission)               
                                                .DocAsUpsert(true));   // si no existe, lo crea (upsert)


            return response.IsValid;
        }
    }
}
