using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.ViewModels
{
    public class MerchantDashboardVM
    {
        public string Month { get; set; }
        public int TransactionCount { get; set; }
        public decimal TransactionValue { get; set; }
    }
}
