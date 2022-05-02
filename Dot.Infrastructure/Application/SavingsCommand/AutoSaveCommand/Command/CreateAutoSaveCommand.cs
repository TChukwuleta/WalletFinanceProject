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

namespace Dot.Infrastructure.Application.SavingsCommand.AutoSaveCommand.Command
{
    public class CreateAutoSaveCommand : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public SavingFrequency SavingFrequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        /*public FundingSource FundingSource { get; set; }*/
        public string Purpose { get; set; }
    }

    public class CreateAutoSaveCommandHandler : IRequestHandler<CreateAutoSaveCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public CreateAutoSaveCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(CreateAutoSaveCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var existingLockAndSave = await _context.Savings.Where(
                    a => a.SavingsType == SavingsType.AutoSave &&
                    a.UserId == request.UserId &&
                    a.Name == request.Name).FirstOrDefaultAsync();
                if (existingLockAndSave != null)
                {
                    return ResultResponse.Failure("An 'Auto Save' Savings already exist with this details");
                }

                var newAutoSave = new Saving
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
                    SavingsType = SavingsType.AutoSave,
                    SavingsTypeDesc = SavingsType.AutoSave.ToString(),
                    SavingFrequency = request.SavingFrequency,
                    SavingFrequencyDesc = request.SavingFrequency.ToString()
                };

                await _context.Savings.AddAsync(newAutoSave);
                await _context.SaveChangesAsync(cancellationToken);
                return ResultResponse.Success($"{request.Name} 'Auto Save' Savings created successfully ", newAutoSave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
