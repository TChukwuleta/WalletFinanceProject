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

namespace Dot.Infrastructure.Application.SavingsCommand.LockAndSaveCommand
{
    public class CreateLockAndSaveCommand : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public int Amount { get; set; }
        public SavingFrequency SavingFrequency { get; set; }
        public DateTime StartDate { get; set; }
        /*public FundingSource FundingSource { get; set; }*/
        public string Purpose { get; set; }
        public DateTime EndDate { get; set; }
    }


    public class CreateLockAndSaveCommandHandler : IRequestHandler<CreateLockAndSaveCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;

        public CreateLockAndSaveCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(CreateLockAndSaveCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var existingLockAndSave = await _context.Savings.Where(a => a.SavingsType == SavingsType.LockAndSave && a.UserId == request.UserId && a.Name == request.Name).FirstOrDefaultAsync();
                if(existingLockAndSave != null)
                {
                    return ResultResponse.Failure("A 'lock N Save' Savings already exist with this details");
                }

                var newLockAndSave = new Saving
                {
                    UserId = request.UserId,
                    Name = request.Name,
                    Duration = request.Duration,
                    StudentId = request.StudentId,
                    Amount = request.Amount,
                    SavingStatus = Core.Enums.SavingStatus.Ongoing,
                    SavingStatusDesc = Core.Enums.SavingStatus.Ongoing.ToString(),
                    Purpose = request.Purpose,
                    StartDate = request.StartDate > DateTime.Now ? request.StartDate : DateTime.Now,
                    EndDate = request.EndDate,
                    FundingSource = FundingSource.Card,
                    FundingSourceDesc = FundingSource.Card.ToString(),
                    SavingsType = SavingsType.LockAndSave,
                    SavingsTypeDesc = SavingsType.LockAndSave.ToString(),
                    SavingFrequency = request.SavingFrequency,
                    SavingFrequencyDesc = request.SavingFrequency.ToString()
                };

                await _context.Savings.AddAsync(newLockAndSave);
                await _context.SaveChangesAsync(cancellationToken);
                return ResultResponse.Success($"{request.Name} Lock N Save Savings created successfully ", newLockAndSave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
