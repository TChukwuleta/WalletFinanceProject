using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Core.Enums;
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
        public UserType UserType { get; set; }
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
                var userWallet = new Wallet();
                switch (request.UserType)
                {
                    case UserType.Student:
                        userWallet = await _context.Wallets.Where(c => c.UserId == request.UserId && c.UserType == UserType.Student).FirstOrDefaultAsync();
                        break;
                    case UserType.Client:
                        userWallet = await _context.Wallets.Where(c => c.UserId == request.UserId && c.UserType == UserType.Client).FirstOrDefaultAsync();
                        break;
                    default:
                        return ResultResponse.Failure("Invalid user type");
                        break;
                }
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
