using Dot.Application.ResponseModel;
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
    public class UpdateSaveAndLockCommand : IRequest<ResultResponse>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string StudentId { get; set; }
        public SavingStatus SavingStatus { get; set; }
        public SavingFrequency SavingFrequency { get; set; }
        public int Amount { get; set; }
        /*public FundingSource FundingSource { get; set; }*/
        public string Purpose { get; set; }
    }

    public class UpdateSaveAndLockCommandHandler : IRequestHandler<UpdateSaveAndLockCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public UpdateSaveAndLockCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(UpdateSaveAndLockCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }
                var existingLockAndSave = await _context.Savings.FirstOrDefaultAsync(
                    a => a.Id == request.Id && 
                    a.UserId == request.UserId && a.StudentId == request.StudentId && 
                    a.SavingsType == SavingsType.LockAndSave);
                if(existingLockAndSave == null)
                {
                    return ResultResponse.Failure($"Lock N Save with Id: {request.Id} not found");
                }

                existingLockAndSave.Name = request.Name;
                existingLockAndSave.Amount = request.Amount;
                existingLockAndSave.SavingStatus = request.SavingStatus;
                existingLockAndSave.Purpose = request.Purpose;
                existingLockAndSave.LastModifiedBy = request.UserId;
                existingLockAndSave.SavingFrequency = request.SavingFrequency;
                existingLockAndSave.SavingFrequencyDesc = request.SavingFrequency.ToString();
                existingLockAndSave.LastModifiedDate = DateTime.Now;

                _context.Savings.Update(existingLockAndSave);
                await _context.SaveChangesAsync(cancellationToken);

                return ResultResponse.Success("Lock N Save updated successfully", existingLockAndSave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
