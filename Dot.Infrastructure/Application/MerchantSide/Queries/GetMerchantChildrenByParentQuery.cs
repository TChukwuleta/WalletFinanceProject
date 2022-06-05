using Dot.Application.ResponseModel;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.MerchantSide.Queries
{
    public class GetMerchantChildrenByParentQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
    }

    public class GetMerchantChildrenByParentQueryHandler : IRequestHandler<GetMerchantChildrenByParentQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetMerchantChildrenByParentQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetMerchantChildrenByParentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findParent = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == request.UserId);
                if(findParent == null)
                {
                    return ResultResponse.Failure("No Merchant found");
                }
                var allChildrenPerThisMerchant = await _context.Clients.Where(c => c.ParentId == findParent.Id).ToListAsync();
                if(allChildrenPerThisMerchant.Count <= 0)
                {
                    return ResultResponse.Failure("Merchant does not have any sublet");
                }
                return ResultResponse.Success("Getting all sublet by merchant was successful", allChildrenPerThisMerchant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
