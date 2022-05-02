using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Core.ViewModels;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.RequestCommand
{
    public class CreateRequestCommand : IRequest<ResultResponse>
    {
        public string ShortDesc { get; set; }
        public string SenderName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Amount { get; set; }
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMailService _mailService;
        public CreateRequestCommandHandler(ApplicationDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }
        public async Task<ResultResponse> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }
                var checkEmail = await _context.Students.FirstOrDefaultAsync(c => c.Email == request.Email);
                if(checkEmail == null)
                {
                    return ResultResponse.Failure("The email inputed does not exist");
                }
                var newRequest = new RequestModel
                {
                    Description = request.ShortDesc,
                    PhoneNumber = request.PhoneNumber,
                    RequestEmail = request.Email,
                    UserId = request.UserId,
                    StudentId = request.StudentId,
                    Amount = request.Amount,
                    RequestDate = DateTime.Now
                };

                var newSendRequest = new MailRequest
                {
                    ToEmail = request.Email,
                    Subject = $"Hey You have got a money request from {findUser.FirstName} {findUser.LastName}",
                    Body = request.ShortDesc
                };

                var sendRequest = _mailService.SendRequestEmailAsync(newSendRequest);

                await _context.RequestModels.AddAsync(newRequest);
                await _context.SaveChangesAsync(cancellationToken);

                return ResultResponse.Success("Request sent successfully", newRequest);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
