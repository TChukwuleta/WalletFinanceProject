using Dot.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string StudentID { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string GenderDesc { get; set; }
        public string UserId { get; set; }
        public Status Status { get; set; }
    }
}
