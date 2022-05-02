using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Entities
{
    public class RequestBuddy
    {
        public int Id { get; set; }
        public string SavingsName { get; set; }
        public string BuddyEmail { get; set; }
        public string BuddyStudentId { get; set; }
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }
}
