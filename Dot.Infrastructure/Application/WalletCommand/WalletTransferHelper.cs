using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Core.Enums;
using Dot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.WalletCommand
{
    internal class WalletTransferHelper
    {
        private readonly ApplicationDbContext _context;
        public WalletTransferHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> WalletTransfer(WalletTransferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return false;
                }
                var findWallet = await _context.Wallets.Where(c => c.WalletAccountNumber == request.RecipientAccountNumber).FirstOrDefaultAsync();
                if (findWallet == null)
                {
                    return false;
                }

                // Handle funds transfer from external party
                findWallet.Balance = findWallet.Balance + request.Amount;
                _context.Wallets.Update(findWallet);
                await _context.SaveChangesAsync(cancellationToken);

                var newTransaction = new Core.Entities.Transaction
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
                    TransactionDate = DateTime.Now,
                    MerchantPaymentType = request.MerchantPaymentType
                };

                await _context.Transactions.AddAsync(newTransaction);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
