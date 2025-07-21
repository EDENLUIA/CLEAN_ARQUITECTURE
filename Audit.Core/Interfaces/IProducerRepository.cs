using Audit.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Core.Interfaces
{
    public interface IProducerRepository
    {
        Task<bool> SendAsync(string topic, OperationEvent operationEvent);
    }
}
