using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Core.Enums;
using Dot.Core.ViewModels;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.SavingsCommand.GroupSavingsCommand.Command
{
    public class CreateGroupSavingsCommand : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public SavingFrequency SavingFrequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<RequestBuddy> SaveBuddies { get; set; }
        /*public FundingSource FundingSource { get; set; }*/
        public string Purpose { get; set; }
    }

    public class CreateGroupSavingsCommandHandler : IRequestHandler<CreateGroupSavingsCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMailService _mailService;
        public CreateGroupSavingsCommandHandler(ApplicationDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }
        public async Task<ResultResponse> Handle(CreateGroupSavingsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var existingLockAndSave = await _context.Savings.Where(
                    a => a.SavingsType == SavingsType.GroupSaving &&
                    a.UserId == request.UserId &&
                    a.Name == request.Name).FirstOrDefaultAsync();
                if (existingLockAndSave != null)
                {
                    return ResultResponse.Failure("An 'Group Saving' Savings already exist with this details");
                }

                if( request.SaveBuddies == null || request.SaveBuddies.Count <= 0)
                {
                    return ResultResponse.Failure("Please input your save buddies");
                }

                var newGroupSaving = new Saving
                {
                    UserId = request.UserId,
                    Name = request.Name,
                    StudentId = request.StudentId,
                    Amount = request.Amount,
                    SavingStatus = SavingStatus.Ongoing,
                    SavingStatusDesc = SavingStatus.Ongoing.ToString(),
                    Purpose = request.Purpose,
                    StartDate = request.StartDate > DateTime.Now ? request.StartDate : DateTime.Now,
                    FundingSource = FundingSource.Card,
                    FundingSourceDesc = FundingSource.Card.ToString(),
                    SavingsType = SavingsType.GroupSaving,
                    SavingsTypeDesc = SavingsType.GroupSaving.ToString(),
                    SavingFrequency = request.SavingFrequency,
                    SavingFrequencyDesc = request.SavingFrequency.ToString()
                };

                var buddyList = new List<RequestBuddy>();
                foreach (var item in request.SaveBuddies)
                {
                    var checkBuddy = await _context.Students.FirstOrDefaultAsync(c => c.Email == item.BuddyEmail && c.StudentID == item.BuddyStudentId);
                    if( checkBuddy == null)
                    {
                        return ResultResponse.Failure($"{item} does not exist on our Application. Kindly replace with another save buddy");
                    }
                    var newBuddy = new RequestBuddy
                    {
                        BuddyEmail = item.BuddyEmail,
                        BuddyStudentId = item.StudentId,
                        SavingsName = request.Name,
                        UserId = request.UserId,
                        StudentId = request.StudentId
                    };
                    buddyList.Add(item);
                }

                newGroupSaving.SaveBuddies = buddyList;

                await _context.Savings.AddAsync(newGroupSaving);
                await _context.SaveChangesAsync(cancellationToken);

                var groupSavingsMail = new MailSaveBuddyRequest();
                foreach (var item in request.SaveBuddies)
                {
                    groupSavingsMail.Email = item.BuddyEmail;
                    groupSavingsMail.Description = request.Purpose;

                    await _mailService.SendSaveBuddyAsync(groupSavingsMail); 
                }
                
                return ResultResponse.Success($"{request.Name} Group Savings created successfully ", newGroupSaving);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
