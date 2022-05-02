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
    public class GetWalletBalance : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GetWalletBalanceHandler : IRequestHandler<GetWalletBalance, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetWalletBalanceHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetWalletBalance request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId);
                if (findUser == null || findUser.Status != Core.Enums.Status.Active)
                {
                    return ResultResponse.Failure("Invalid active user");
                }
                var userWallet = await _context.Wallets.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.StudentId == request.StudentId);
                if (userWallet == null)
                {
                    return ResultResponse.Failure("Invalid wallet user specified");
                }

                // Get balance from third party
                return ResultResponse.Success(userWallet.Balance);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
