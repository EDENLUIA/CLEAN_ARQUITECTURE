using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Core
{
    public class Permission
    {      
        public int Id { get; set; }

        public string? EmployeeForename { get; set; }

        public string? EmployeeSurname { get; set; }

        public int PermissionType { get; set; }

        public DateTime PermissionDate { get; set; }
    }
}
