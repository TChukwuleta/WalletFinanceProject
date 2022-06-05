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

namespace Dot.Infrastructure.Application.WalletQueries
{
    public class GetWalletBalance : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public UserType UserType { get; set; }
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
                switch (request.UserType)
                {
                    case UserType.Student:
                        var findStudent = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                        if (findStudent == null)
                        {
                            return ResultResponse.Failure("Invalid active user");
                        }
                        var findStudentWallet = await _context.Wallets.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.UserType == UserType.Student);
                        if (findStudentWallet == null)
                        {
                            return ResultResponse.Failure("Invalid wallet user specified");
                        }
                        // Get balance from third party
                        return ResultResponse.Success(findStudentWallet.Balance);
                        break;
                    case UserType.Client:
                        var findClient = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                        if (findClient == null)
                        {
                            return ResultResponse.Failure("Invalid active client");
                        }
                        var findClientWallet = await _context.Wallets.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.UserType == UserType.Client);
                        if (findClientWallet == null)
                        {
                            return ResultResponse.Failure("Invalid wallet user specified");
                        }
                        // Get balance from third party
                        return ResultResponse.Success(findClientWallet.Balance);
                        break;
                    default:
                        break;
                }
                return ResultResponse.Success("Retrieving wallet balance was successful");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
