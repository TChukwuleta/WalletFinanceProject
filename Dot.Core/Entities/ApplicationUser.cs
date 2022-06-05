using Dot.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentID { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public string GenderDesc { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }

        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string BusinessRegNo { get; set; }
        public string Role { get; set; }
        public string ParentName { get; set; }
    }
}
