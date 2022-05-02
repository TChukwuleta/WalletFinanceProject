using Dot.Application.ResponseModel;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.WalletQueries
{
    public class GetWalletDetails : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GetWalletDetailsHandler : IRequestHandler<GetWalletDetails, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetWalletDetailsHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetWalletDetails request, CancellationToken cancellationToken)
        {
            try
            {
                var userWallet = await _context.Wallets.Where(c => c.UserId == request.UserId && c.StudentId == request.StudentId).FirstOrDefaultAsync();
                if (userWallet == null)
                {
                    return ResultResponse.Failure("Invalid wallet user");
                }

                return ResultResponse.Success(userWallet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
