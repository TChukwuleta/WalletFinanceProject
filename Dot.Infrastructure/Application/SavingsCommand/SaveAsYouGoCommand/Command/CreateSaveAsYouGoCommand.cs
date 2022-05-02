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

namespace Dot.Infrastructure.Application.SavingsCommand.SaveAsYouGoCommand.Command
{
    public class CreateSaveAsYouGoCommand : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public SavingFrequency SavingFrequency { get; set; }
        public DateTime StartDate { get; set; }
        /*public FundingSource FundingSource { get; set; }*/
        public string Purpose { get; set; }
    }

    public class CreateSaveAsYouGoCommandHandler : IRequestHandler<CreateSaveAsYouGoCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;

        public CreateSaveAsYouGoCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(CreateSaveAsYouGoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var existingLockAndSave = await _context.Savings.Where(
                    a => a.SavingsType == SavingsType.SaveAsYouGo && 
                    a.UserId == request.UserId && 
                    a.Name == request.Name).FirstOrDefaultAsync();
                if (existingLockAndSave != null)
                {
                    return ResultResponse.Failure("A 'Save as you go' Savings already exist with this details");
                }

                var newSaveAsYouGo = new Saving
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
                    SavingsType = SavingsType.SaveAsYouGo,
                    SavingsTypeDesc = SavingsType.SaveAsYouGo.ToString(),
                    SavingFrequency = request.SavingFrequency,
                    SavingFrequencyDesc = request.SavingFrequency.ToString()
                };

                await _context.Savings.AddAsync(newSaveAsYouGo);
                await _context.SaveChangesAsync(cancellationToken);
                return ResultResponse.Success($"{request.Name} 'Save as you go' Savings created successfully ", newSaveAsYouGo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
