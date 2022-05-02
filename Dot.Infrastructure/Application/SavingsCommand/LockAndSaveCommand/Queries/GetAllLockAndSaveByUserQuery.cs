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
    public class GetAllLockAndSaveByUserQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GetAllLockAndSaveByUserQueryHandler : IRequestHandler<GetAllLockAndSaveByUserQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetAllLockAndSaveByUserQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetAllLockAndSaveByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var lockAndSave = await _context.Savings.Where(
                    c => c.UserId == request.UserId &&
                    c.StudentId == request.StudentId
                    && c.SavingsType == Core.Enums.SavingsType.LockAndSave).ToListAsync();
                if (lockAndSave == null)
                {
                    return ResultResponse.Failure("YOu don't hav any 'Lock N Save'");
                }

                return ResultResponse.Success(lockAndSave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}