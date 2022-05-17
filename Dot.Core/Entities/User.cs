using Dot.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Entities
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string SchoolName { get; set; }
        public string Address { get; set; }
        public string StudentID { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
    }
}
