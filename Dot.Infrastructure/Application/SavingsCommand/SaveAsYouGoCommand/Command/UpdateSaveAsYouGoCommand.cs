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

namespace Dot.Infrastructure.Application.SavingsCommand.SaveAsYouGoCommand.Command
{
    public class UpdateSaveAsYouGoCommand : IRequest<ResultResponse>
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

    public class UpdateSaveAsYouGoCommandHandler : IRequestHandler<UpdateSaveAsYouGoCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;

        public UpdateSaveAsYouGoCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(UpdateSaveAsYouGoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }
                var existingSaveAsYouGo = await _context.Savings.FirstOrDefaultAsync(
                    a => a.Id == request.Id && 
                    a.UserId == request.UserId && 
                    a.StudentId == request.StudentId &&
                    a.SavingsType == SavingsType.SaveAsYouGo);
                if (existingSaveAsYouGo == null)
                {
                    return ResultResponse.Failure($"Lock N Save with Id: {request.Id} not found");
                }

                existingSaveAsYouGo.Name = request.Name;
                existingSaveAsYouGo.Amount = request.Amount;
                existingSaveAsYouGo.SavingStatus = request.SavingStatus;
                existingSaveAsYouGo.Purpose = request.Purpose;
                existingSaveAsYouGo.LastModifiedBy = request.UserId;
                existingSaveAsYouGo.SavingFrequency = request.SavingFrequency;
                existingSaveAsYouGo.SavingFrequencyDesc = request.SavingFrequency.ToString();
                existingSaveAsYouGo.LastModifiedDate = DateTime.Now;

                _context.Savings.Update(existingSaveAsYouGo);
                await _context.SaveChangesAsync(cancellationToken);

                return ResultResponse.Success("'Save as you go' updated successfully", existingSaveAsYouGo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
