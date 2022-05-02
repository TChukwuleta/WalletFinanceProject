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

namespace Dot.Infrastructure.Application.SavingsCommand.AutoSaveCommand.Command
{
    public class UpdateAutoSaveCommand : IRequest<ResultResponse>
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

    public class UpdateAutoSaveCommandHandler : IRequestHandler<UpdateAutoSaveCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public UpdateAutoSaveCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(UpdateAutoSaveCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }
                var existingAutoSave = await _context.Savings.FirstOrDefaultAsync(
                    a => a.Id == request.Id &&
                    a.UserId == request.UserId &&
                    a.StudentId == request.StudentId &&
                    a.SavingsType == SavingsType.AutoSave);
                if (existingAutoSave == null)
                {
                    return ResultResponse.Failure($"Auto Save with Id: {request.Id} not found");
                }

                existingAutoSave.Name = request.Name;
                existingAutoSave.Amount = request.Amount;
                existingAutoSave.SavingStatus = request.SavingStatus;
                existingAutoSave.Purpose = request.Purpose;
                existingAutoSave.LastModifiedBy = request.UserId;
                existingAutoSave.SavingFrequency = request.SavingFrequency;
                existingAutoSave.SavingFrequencyDesc = request.SavingFrequency.ToString();
                existingAutoSave.LastModifiedDate = DateTime.Now;

                _context.Savings.Update(existingAutoSave);
                await _context.SaveChangesAsync(cancellationToken);

                return ResultResponse.Success("'Auto Save' updated successfully", existingAutoSave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
