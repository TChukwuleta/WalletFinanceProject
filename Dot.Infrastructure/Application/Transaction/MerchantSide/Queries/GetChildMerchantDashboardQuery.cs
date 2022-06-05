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
    public class GetChildMerchantDashboardQuery : IRequest<ResultResponse>
    {
        public string MerchantUserId { get; set; }
        public string StoreUserId { get; set; }
    }
    public class GetChildMerchantDashboardQueryHandler : IRequestHandler<GetChildMerchantDashboardQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetChildMerchantDashboardQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetChildMerchantDashboardQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findChildMerchant = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == request.StoreUserId);
                if(findChildMerchant == null)
                {
                    return ResultResponse.Failure("Child store does not exist");
                }

                var findChildMerchantTxns = await _context.Transactions.Where(c => c.UserId == findChildMerchant.UserId).ToListAsync();
                if(findChildMerchantTxns.Count() <= 0)
                {
                    return ResultResponse.Failure("No transaction has been made");
                }

                var transactionCountResult = new
                {
                    SupplierPaymentTransactionCount = findChildMerchantTxns.Where(c => c.MerchantPaymentType == MerchantPaymentType.SupplierPayment).Count(),
                    TaxTransactionCount = findChildMerchantTxns.Where(c => c.MerchantPaymentType == MerchantPaymentType.Tax).Count(),
                    PayrollTransactionCount = findChildMerchantTxns.Where(c => c.MerchantPaymentType == MerchantPaymentType.Payroll).Count(),
                    RefundTransactionCount = findChildMerchantTxns.Where(c => c.MerchantPaymentType == MerchantPaymentType.Refund).Count(),
                    BillTransactionCount = findChildMerchantTxns.Where(c => c.MerchantPaymentType == MerchantPaymentType.Bill).Count()
                };

                var transactionValueResult = new
                {
                    SupplierPaymentTransactionValue = findChildMerchantTxns.Where(c => c.MerchantPaymentType == MerchantPaymentType.SupplierPayment).Sum(c => c.Amount),
                    TraxTransactionValue = findChildMerchantTxns.Where(c => c.MerchantPaymentType == MerchantPaymentType.Tax).Sum(c => c.Amount),
                    PayrollTransactionValue = findChildMerchantTxns.Where(c => c.MerchantPaymentType == MerchantPaymentType.Payroll).Sum(c => c.Amount),
                    RefundTransactionValue = findChildMerchantTxns.Where(c => c.MerchantPaymentType == MerchantPaymentType.Refund).Sum(c => c.Amount),
                    BillTransactionValue = findChildMerchantTxns.Where(c => c.MerchantPaymentType == MerchantPaymentType.Bill).Sum(c => c.Amount)
                };

                return ResultResponse.Success(new { transactionCountResult, transactionValueResult });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
