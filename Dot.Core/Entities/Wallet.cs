using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public string WalletAccountNumber { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal Balance { get; set; }
        public string StudentId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
