using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcePlanning.Models
{
    public class GridUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string RoleId { get; set; }
        //public virtual GridRoleViewModel Role { get; set; }

    }
}
