using Dot.Application.ResponseModel;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.SavingsCommand.GroupSavingsCommand.Queries
{
    public class GetAllActiveGroupSavingByUserQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GetAllActiveGroupSavingByUserQueryHandler : IRequestHandler<GetAllActiveGroupSavingByUserQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetAllActiveGroupSavingByUserQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetAllActiveGroupSavingByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var activeGroupSavings = await _context.Savings.Where(
                    c => c.UserId == request.UserId &&
                    c.StudentId == request.StudentId
                    && c.SavingsType == Core.Enums.SavingsType.GroupSaving &&
                    c.SavingStatus == Core.Enums.SavingStatus.Ongoing).ToListAsync();
                if (activeGroupSavings == null)
                {
                    return ResultResponse.Failure("YOu don't hav any Active Group Savings");
                }

                return ResultResponse.Success(activeGroupSavings);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}