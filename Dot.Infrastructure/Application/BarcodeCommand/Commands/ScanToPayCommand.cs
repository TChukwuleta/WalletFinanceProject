using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using Dot.Core.Enums;
using Dot.Core.ViewModels;
using Dot.Infrastructure.Application.WalletCommand;
using Dot.Infrastructure.Data;
using IronBarCode;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.BarcodeCommand.Commands
{
    public class ScanToPayCommand : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public string Image { get; set; }
        public string Extension { get; set; }
        public MerchantPaymentType MerchantPaymentType { get; set; }
    }

    public class ScanToPayCommandHandler : IRequestHandler<ScanToPayCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUploadService _uploadService;
        public ScanToPayCommandHandler(ApplicationDbContext context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }
        public async Task<ResultResponse> Handle(ScanToPayCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var filename = $"{findUser.FirstName}_{findUser.UserId}.{request.Extension}";
                var getFile = await _uploadService.FromBase64ToFile(request.Image, filename);
                if (getFile == null)
                {
                    return ResultResponse.Failure($"An error occured while converting base64 to a(n) {request.Extension} format");
                }
                var result = BarcodeReader.QuicklyReadOneBarcode(getFile);
                if (result == null)
                {
                    return ResultResponse.Failure("Error occured while reading image to Barcode");
                }
                var getInfo = JsonSerializer.Deserialize<fileDetailsRequest>(result.Text);

                var findWalletUser = await _context.Wallets.Where(c => c.WalletAccountNumber == getInfo.AccountNumber).FirstOrDefaultAsync();
                if (findWalletUser == null)
                {
                    return ResultResponse.Failure("Merchant user wallet does not exist");
                }

                var transferCommand = new WalletTransferCommand
                {
                    UserId = request.UserId,
                    StudentId = request.StudentId,
                    RecipientAccountNumber = findWalletUser.WalletAccountNumber,
                    RecipientName = findWalletUser.UserName,
                    TransactionType = TransactionType.Debit,
                    CurrencyCode = CurrencyCode.NGN,
                    Narration = ""
                };

                var transferCommandHandler = new WalletTransferHelper(_context);
                var payToMerchant = await transferCommandHandler.WalletTransfer(transferCommand, cancellationToken);
                if (!payToMerchant)
                {
                    return ResultResponse.Failure("Transfer was not successful");
                }

                return ResultResponse.Success("Transfer was successful", payToMerchant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
