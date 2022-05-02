using Dot.Application.ResponseModel;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.SavingsCommand
{
    public class GetAllSavingsByUserQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GetAllSavingsByUserQueryHandler : IRequestHandler<GetAllSavingsByUserQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetAllSavingsByUserQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetAllSavingsByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var getAllSavings = await _context.Savings.Where(a => a.UserId == request.UserId && a.StudentId == request.StudentId).ToListAsync();
                if (getAllSavings == null)
                {
                    return ResultResponse.Failure("You dont have any savings product");
                }

                return ResultResponse.Success(getAllSavings);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
