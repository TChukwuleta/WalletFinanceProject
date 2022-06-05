using Dot.Application.ResponseModel;
using Dot.Core.Enums;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.Transaction.MerchantSide.Queries
{
    public class GetChildMerchantDashboartByPaymentTypeQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public MerchantPaymentType PaymentType { get; set; }
    }

    public class GetChildMerchantDashboartByPaymentTypeQueryHandler : IRequestHandler<GetChildMerchantDashboartByPaymentTypeQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetChildMerchantDashboartByPaymentTypeQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetChildMerchantDashboartByPaymentTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findMerchant = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == request.UserId);
                if(findMerchant == null)
                {
                    return ResultResponse.Failure("Merchant does not exist");
                }

                var getMerchantTransactions = await _context.Transactions.Where(c => c.UserId == findMerchant.UserId).ToListAsync();
                if(getMerchantTransactions.Count() <= 0)
                {
                    return ResultResponse.Failure("No transaction found for this Merchant");
                }

                switch (request.PaymentType)
                {
                    case MerchantPaymentType.DoesNotApply:
                        return ResultResponse.Failure("Nothing to return");
                        break;
                    case MerchantPaymentType.SupplierPayment:
                        var supplierPaymentTransactionCount = getMerchantTransactions.Where(c => c.MerchantPaymentType == MerchantPaymentType.SupplierPayment).Count();
                        var supplierPaymentTransactionValue = getMerchantTransactions.Where(c => c.MerchantPaymentType == MerchantPaymentType.SupplierPayment).Sum(c => c.Amount);
                        return ResultResponse.Success(new { supplierPaymentTransactionCount, supplierPaymentTransactionValue });
                        break;
                    case MerchantPaymentType.Tax:
                        var taxTransactionCount = getMerchantTransactions.Where(c => c.MerchantPaymentType == MerchantPaymentType.Tax).Count();
                        var taxTransactionValue = getMerchantTransactions.Where(c => c.MerchantPaymentType == MerchantPaymentType.Tax).Sum(c => c.Amount);
                        return ResultResponse.Success(new { taxTransactionCount, taxTransactionValue });
                        break;
                    case MerchantPaymentType.Payroll:
                        var payrollTransactionCount = getMerchantTransactions.Where(c => c.MerchantPaymentType == MerchantPaymentType.Payroll).Count();
                        var payrollTransactionValue = getMerchantTransactions.Where(c => c.MerchantPaymentType == MerchantPaymentType.Payroll).Sum(c => c.Amount);
                        return ResultResponse.Success(new { payrollTransactionCount, payrollTransactionValue });
                        break;
                    case MerchantPaymentType.Refund:
                        var refundTransactionCount = getMerchantTransactions.Where(c => c.MerchantPaymentType == MerchantPaymentType.Refund).Count();
                        var refundTransactionValue = getMerchantTransactions.Where(c => c.MerchantPaymentType == MerchantPaymentType.Refund).Sum(c => c.Amount);
                        return ResultResponse.Success(new { refundTransactionCount, refundTransactionValue });
                        break;
                    case MerchantPaymentType.Bill:
                        var billTransactionCount = getMerchantTransactions.Where(c => c.MerchantPaymentType == MerchantPaymentType.Bill).Count();
                        var billTransactionValue = getMerchantTransactions.Where(c => c.MerchantPaymentType == MerchantPaymentType.Bill).Sum(c => c.Amount);
                        return ResultResponse.Success(new { billTransactionCount, billTransactionValue });
                        break;
                    default:
                        return ResultResponse.Failure("Invalid payment type");
                        break;
                }

                return ResultResponse.Success("Sucessful");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
