using CloudinaryDotNet;
using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Core.ViewModels;
using Dot.Infrastructure.Data;
using IronBarCode;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.BarcodeCommand.Commands
{
    public class GenerateBarcodeCommand : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GenerateBarcodeCommandHandler : IRequestHandler<GenerateBarcodeCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUploadService _uploadService;

        public GenerateBarcodeCommandHandler(ApplicationDbContext context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }
        public async Task<ResultResponse> Handle(GenerateBarcodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }


                /*var findUserBarcode = await _context.Barcodes.FirstOrDefaultAsync(a => a.UserId == request.UserId && a.StudentId == request.StudentId);    
                if(findUserBarcode != null)
                {
                    return ResultResponse.Success(findUserBarcode);
                }*/

                var walletUser = await _context.Wallets.FirstOrDefaultAsync(v => v.UserId == request.UserId && v.StudentId == request.StudentId);
                if(walletUser == null)
                {
                    return ResultResponse.Failure("Please create a wallet");
                }

                var newBarcode = new Barcode
                {
                    Name = $"{walletUser.UserName}",
                    AccountNumber = walletUser.WalletAccountNumber,
                    UserId = request.UserId,
                    StudentId = request.StudentId,
                    CreatedDate = DateTime.Now,
                    BarcodeUrl = ""

                };

                await _context.Barcodes.AddAsync(newBarcode);
                await _context.SaveChangesAsync(cancellationToken);

                var barcodeDetails = new
                {
                    Name = $"{walletUser.UserName}",
                    AccountNumber = walletUser.WalletAccountNumber,
                };


                var serializeObject = JsonSerializer.Serialize(barcodeDetails);
                //GeneratedBarcode QrCode = QRCodeWriter.CreateQrCode(serializeObject).SaveAsPng($"{newBarcode.Name}_{newBarcode.StudentId}.png");

                var QrCode = BarcodeWriter.CreateBarcode(serializeObject, BarcodeWriterEncoding.QRCode).SaveAsJpeg($"{newBarcode.Name}_{newBarcode.UserId}.jpg");

                var fileUrl = await _uploadService.UploadImage(newBarcode.Name, newBarcode.UserId);

                var findBarcode = await _context.Barcodes.Where(c => c.UserId == request.UserId && c.StudentId == request.StudentId).FirstOrDefaultAsync();

                findBarcode.BarcodeUrl = fileUrl;

                _context.Update(findBarcode);
                await _context.SaveChangesAsync(cancellationToken);

                return ResultResponse.Success(findBarcode);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
