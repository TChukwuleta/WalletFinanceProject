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
    public class GetMerchantByIdQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
    }

    public class GetMerchantByIdQueryHandler : IRequestHandler<GetMerchantByIdQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetMerchantByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetMerchantByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findMerchant = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == request.UserId);
                if(findMerchant == null)
                {
                    return ResultResponse.Failure("No Client with that Id exist");
                }
                return ResultResponse.Success(findMerchant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
