using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Core.Common
{
    public class OperationEvent
    {
        public OperationEvent(Guid guid, string typeOperation)
        {
            Guid = guid;
            TypeOperation = typeOperation;
        }

        public Guid Guid { get; set; }
        public string TypeOperation { get; set; }
    }
}
