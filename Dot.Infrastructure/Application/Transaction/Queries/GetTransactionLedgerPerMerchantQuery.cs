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
    public class GetTransactionLedgerPerMerchantQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetTransactionLedgerPerMerchantQueryHandler : IRequestHandler<GetTransactionLedgerPerMerchantQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetTransactionLedgerPerMerchantQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetTransactionLedgerPerMerchantQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findParent = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == request.UserId);
                if(findParent == null)
                {
                    return ResultResponse.Failure("No Merchant found");
                }

                var allMerchantStores = await _context.Clients.Where(c => c.ParentId == findParent.Id).ToListAsync();
                if(allMerchantStores.Count() <= 0)
                {
                    return ResultResponse.Failure("Merchant does not have any store");
                }

                var merchantLedger = new List<TestClass>();
                foreach (var item in allMerchantStores)
                {
                    var childTestClass = new TestClass();
                    var childTransactions = await _context.Transactions.Where(c => c.UserId == item.UserId).ToListAsync();
                    var childDashboard = new List<MerchantDashboardVM>();
                    foreach (Month month in Enum.GetValues(typeof(Month)))
                    {
                        var childDetails = new MerchantDashboardVM();
                        childDetails.Month = month.ToString();
                        childDetails.TransactionCount = childTransactions.Where(c => c.TransactionDate.Month == (int)month).Count();
                        childDetails.TransactionValue = childTransactions.Where(c => c.TransactionDate.Month == (int)month).Sum(c => c.Amount);
                        childDashboard.Add(childDetails);
                    }
                    childTestClass.ChildDetails = childDashboard;
                    childTestClass.ChildName = item.FullName;
                    merchantLedger.Add(childTestClass);
                }

                if(request.Skip == 0 && request.Take == 0)
                {
                    var merchantLedgerResult = new
                    {
                        MerchantLedger = merchantLedger,
                        Count = merchantLedger.Count()
                    };
                    return ResultResponse.Success(merchantLedgerResult);
                }
                var truncatedLedgerResult = new
                {
                    MerchantLedger = merchantLedger.Skip(request.Skip).Take(request.Take),
                    Count = merchantLedger.Count()
                };
                return ResultResponse.Success(truncatedLedgerResult);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public class TestClass
        {
            public string ChildName { get; set; }
            public List<MerchantDashboardVM> ChildDetails { get; set; }
        }
    }
}
