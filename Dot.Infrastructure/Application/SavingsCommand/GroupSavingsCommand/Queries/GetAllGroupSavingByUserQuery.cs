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
    public class GetAllGroupSavingByUserQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GetAllGroupSavingByUserQueryHandler : IRequestHandler<GetAllGroupSavingByUserQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetAllGroupSavingByUserQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetAllGroupSavingByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var groupSavings = await _context.Savings.Where(
                    c => c.UserId == request.UserId &&
                    c.StudentId == request.StudentId
                    && c.SavingsType == Core.Enums.SavingsType.GroupSaving).ToListAsync();
                if (groupSavings == null)
                {
                    return ResultResponse.Failure("YOu don't hav any Group Savings Savings");
                }

                return ResultResponse.Success(groupSavings);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}