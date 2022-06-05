using Dot.Application.ResponseModel;
using Dot.Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.Transaction.Queries
{
    public class RevenueValuePerMerchantQuery : IRequest<ResultResponse>
    {
        public string ParentMerchantUserId { get; set; }
    }

    public class RevenueValuePerMerchantQueryHandler : IRequestHandler<RevenueValuePerMerchantQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public RevenueValuePerMerchantQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<ResultResponse> Handle(RevenueValuePerMerchantQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
