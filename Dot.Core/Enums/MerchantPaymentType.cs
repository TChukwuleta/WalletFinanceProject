using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Enums
{
    public enum MerchantPaymentType
    {
        DoesNotApply = 1,
        SupplierPayment,
        Tax,
        Payroll,
        Refund,
        Bill
    }
}
