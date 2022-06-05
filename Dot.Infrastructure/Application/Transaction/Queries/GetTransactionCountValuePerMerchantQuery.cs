using Dot.Application.ResponseModel;
using Dot.Core.Enums;
using Dot.Core.ViewModels;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.Transaction.Queries
{
    public class GetTransactionCountValuePerMerchantQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
    }

    public class GetTransactionCountValuePerMerchantQueryHandler : IRequestHandler<GetTransactionCountValuePerMerchantQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetTransactionCountValuePerMerchantQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetTransactionCountValuePerMerchantQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findMerchant = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == request.UserId);
                if(findMerchant == null)
                {
                    return ResultResponse.Failure("Merchant not found");
                }

                var allChildrenTransactionCount = await _context.Transactions.Where(c => c.ParentId == findMerchant.Id && c.UserType == Core.Enums.UserType.Client).ToListAsync();
                if(allChildrenTransactionCount.Count() <= 0)
                {
                    return ResultResponse.Failure("No Transaction available for this merchant");
                }

                var merchantDashboard = new List<MerchantDashboardVM>();
                foreach (Month month in Enum.GetValues(typeof(Month)))
                {
                    var dashboard = new MerchantDashboardVM();
                    dashboard.Month = month.ToString();
                    dashboard.TransactionCount = allChildrenTransactionCount.Where(c => c.TransactionDate.Month == (int)month).Count();
                    dashboard.TransactionValue = allChildrenTransactionCount.Where(c => c.TransactionDate.Month == (int)month).Sum(c => c.Amount);
                    merchantDashboard.Add(dashboard);
                }

                return ResultResponse.Success("Retrireving Transaction details per Merchant was successful", merchantDashboard);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
