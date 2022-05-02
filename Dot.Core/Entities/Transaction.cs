using Dot.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string WalletNumber { get; set; }
        public string StudentId { get; set; }
        public string UserId { get; set; }
        public string RecipientName { get; set; }
        public string RecipientAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public string TransactionStatusDesc { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string Narration { get; set; }
        public string TransactionReference { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
