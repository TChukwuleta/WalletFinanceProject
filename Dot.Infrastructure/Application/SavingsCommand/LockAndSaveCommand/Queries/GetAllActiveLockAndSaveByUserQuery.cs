using Dot.Application.ResponseModel;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.SavingsCommand.LockAndSaveCommand.Queries
{
    public class GetAllActiveLockAndSaveByUserQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GetAllActiveLockAndSaveByUserQueryHandler : IRequestHandler<GetAllActiveLockAndSaveByUserQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetAllActiveLockAndSaveByUserQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetAllActiveLockAndSaveByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var activeLockAndSave = await _context.Savings.Where(
                    c => c.UserId == request.UserId && 
                    c.StudentId == request.StudentId 
                    && c.SavingsType == Core.Enums.SavingsType.LockAndSave && 
                    c.SavingStatus == Core.Enums.SavingStatus.Ongoing).ToListAsync();
                if(activeLockAndSave == null)
                {
                    return ResultResponse.Failure("YOu don't hav any active Lock N Save");
                }

                return ResultResponse.Success(activeLockAndSave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
