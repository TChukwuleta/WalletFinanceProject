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
    public class ExtendLockAndSaveCommand : IRequest<ResultResponse>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public int AdditionalAmount { get; set; }
        public DateTime NewEndDate { get; set; }
    }

    public class ExtendLockAndSaveCommandHandler : IRequestHandler<ExtendLockAndSaveCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;

        public ExtendLockAndSaveCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(ExtendLockAndSaveCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var existingSaveAndLock = await _context.Savings.Where(
                    a => a.UserId == request.UserId && 
                    a.Id == request.Id && 
                    a.StudentId == request.StudentId && 
                    a.SavingsType == SavingsType.LockAndSave).FirstOrDefaultAsync();
                if(existingSaveAndLock == null)
                {
                    return ResultResponse.Failure($"Lock N Save with Id: {request.Id} not found");
                }

                if(existingSaveAndLock.StartDate.Subtract(DateTime.Now).TotalDays > 0)
                {
                    return ResultResponse.Failure("Your Lock N Save is yet to start");
                }

                if(request.NewEndDate.Subtract(existingSaveAndLock.EndDate).TotalDays <= 0)
                {
                    return ResultResponse.Failure("End date has already elapsed");
                }

                existingSaveAndLock.EndDate = request.NewEndDate;
                existingSaveAndLock.LastModifiedDate = DateTime.Now;
                existingSaveAndLock.LastModifiedBy = request.UserId;

                if(request.AdditionalAmount > 0)
                {
                    existingSaveAndLock.Amount = existingSaveAndLock.Amount + request.AdditionalAmount;
                }

                _context.Savings.Update(existingSaveAndLock);
                await _context.SaveChangesAsync(cancellationToken);

                return ResultResponse.Success("Save N Lock extended successfully", existingSaveAndLock);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
