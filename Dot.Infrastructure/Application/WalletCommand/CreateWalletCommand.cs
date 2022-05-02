using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.WalletCommand
{
    public class CreateWalletCommand : IRequest<ResultResponse>
    {
        public string StudentId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountName { get; set; }
        public string UserId { get; set; }
    }

    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public CreateWalletCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }
                // Creating dynamic account
                Random rnd = new Random(DateTime.Now.Millisecond);
                int key = 0;
                key = rnd.Next(1000000000, int.MaxValue);

                var newWallet = new Wallet
                {
                    WalletAccountNumber = key.ToString(),
                    ClosingBalance = 0,
                    Balance = 0,
                    StudentId = request.StudentId,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    UserId = request.UserId,
                    UserName = $"{findUser.FirstName} {findUser.LastName}" // Let user name be a combo of first and last name
                };

                await _context.Wallets.AddAsync(newWallet);
                await _context.SaveChangesAsync(cancellationToken);

                return ResultResponse.Success("User wallet created successfully", newWallet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
