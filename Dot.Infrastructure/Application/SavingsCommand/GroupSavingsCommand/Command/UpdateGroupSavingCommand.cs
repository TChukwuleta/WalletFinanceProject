using Dot.Application.ResponseModel;
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
    public class UpdateGroupSavingCommand : IRequest<ResultResponse>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string StudentId { get; set; }
        public SavingStatus SavingStatus { get; set; }
        public SavingFrequency SavingFrequency { get; set; }
        public int Amount { get; set; }
        public string Purpose { get; set; }
    }

    public class UpdateGroupSavingCommandHandler : IRequestHandler<UpdateGroupSavingCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public UpdateGroupSavingCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(UpdateGroupSavingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }
                var existingGroupSaving = await _context.Savings.FirstOrDefaultAsync(
                    a => a.Id == request.Id &&
                    a.UserId == request.UserId &&
                    a.StudentId == request.StudentId &&
                    a.SavingsType == SavingsType.GroupSaving);
                if (existingGroupSaving == null)
                {
                    return ResultResponse.Failure($"Group Savings with Id: {request.Id} not found");
                }

                existingGroupSaving.Name = request.Name;
                existingGroupSaving.Amount = request.Amount;
                existingGroupSaving.SavingStatus = request.SavingStatus;
                existingGroupSaving.Purpose = request.Purpose;
                existingGroupSaving.LastModifiedBy = request.UserId;
                existingGroupSaving.SavingFrequency = request.SavingFrequency;
                existingGroupSaving.SavingFrequencyDesc = request.SavingFrequency.ToString();
                existingGroupSaving.LastModifiedDate = DateTime.Now;

                _context.Savings.Update(existingGroupSaving);
                await _context.SaveChangesAsync(cancellationToken);

                return ResultResponse.Success("'Group Saving' updated successfully", existingGroupSaving);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
