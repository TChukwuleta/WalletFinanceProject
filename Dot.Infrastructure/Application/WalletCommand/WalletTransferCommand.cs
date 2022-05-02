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

namespace Dot.Infrastructure.Application.WalletCommand
{
    public class WalletTransferCommand : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public decimal Amount { get; set; }
        public string RecipientAccountNumber { get; set; }
        public string RecipientName { get; set; }
        public TransactionType TransactionType { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string Narration { get; set; }
    }

    public class WalletTransferCommandHandler : IRequestHandler<WalletTransferCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;

        public WalletTransferCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(WalletTransferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId);
                if (findUser == null || findUser.Status != Core.Enums.Status.Active)
                {
                    return ResultResponse.Failure("Invalid active user");
                }
                var findWallet = await _context.Wallets.Where(c => c.WalletAccountNumber == request.RecipientAccountNumber).FirstOrDefaultAsync();
                if (findWallet == null)
                {
                    return ResultResponse.Failure("Invalid beneficiary account number");
                }

                // Handle funds transfer from external party
                findWallet.Balance = findWallet.Balance + request.Amount;
                _context.Wallets.Update(findWallet);
                await _context.SaveChangesAsync(cancellationToken);

                var newTransaction = new Transaction
                {
                    WalletNumber = findWallet.WalletAccountNumber, // To Be sorted
                    StudentId = request.StudentId,
                    UserId = request.UserId,
                    RecipientName = request.RecipientName,
                    RecipientAccountNumber = request.RecipientAccountNumber,
                    Amount = request.Amount,
                    TransactionStatus = TransactionStatus.Processing,
                    TransactionStatusDesc = TransactionStatus.Processing.ToString(),
                    CurrencyCode = request.CurrencyCode,
                    Narration = request.Narration,
                    TransactionReference = "", // To be sorted.
                    TransactionDate = DateTime.Now
                };

                await _context.Transactions.AddAsync(newTransaction);
                await _context.SaveChangesAsync(cancellationToken);

                return ResultResponse.Success("Transfer was successful");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
