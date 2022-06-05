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
    public class GetChildMerchantTransactionByPaymentTypeQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public MerchantPaymentType PaymentType { get; set; }
    }

    public class GetChildMerchantTransactionByPaymentTypeQueryHandler : IRequestHandler<GetChildMerchantTransactionByPaymentTypeQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetChildMerchantTransactionByPaymentTypeQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetChildMerchantTransactionByPaymentTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findMerchant = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == request.UserId);
                if(findMerchant == null)
                {
                    return ResultResponse.Failure("Merchant does not exist");
                }
                var getPaymentTypeTransactions = await _context.Transactions.Where(c => c.UserId == findMerchant.UserId && c.MerchantPaymentType == request.PaymentType).ToListAsync();
                if(getPaymentTypeTransactions.Count() <= 0)
                {
                    return ResultResponse.Failure($"No transaction found for {request.PaymentType.ToString()}");
                }
                return ResultResponse.Success("Retrieving Payment type transactions was successful", getPaymentTypeTransactions);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
