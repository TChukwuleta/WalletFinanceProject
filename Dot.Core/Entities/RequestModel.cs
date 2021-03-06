using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Entities
{
    public class RequestModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string RequestEmail { get; set; }
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public int Amount { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
