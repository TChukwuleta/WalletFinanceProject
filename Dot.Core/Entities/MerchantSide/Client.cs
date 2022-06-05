using Dot.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Entities.MerchantSide
{
    public class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string BusinessRegNo { get; set; }
        public string Role { get; set; }
        public ComapnySector ComapnySector { get; set; }
        public string UserId { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public int ParentId { get; set; }
        public string ParentFullName { get; set; }
    }
}
