using Dot.Application.ResponseModel;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.SavingsCommand.AutoSaveCommand.Queries
{
    public class GetAllActiveAutoSaveQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GetAllActiveAutoSaveQueryHandler : IRequestHandler<GetAllActiveAutoSaveQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetAllActiveAutoSaveQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetAllActiveAutoSaveQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var activeAutoSave = await _context.Savings.Where(
                    c => c.UserId == request.UserId &&
                    c.StudentId == request.StudentId
                    && c.SavingsType == Core.Enums.SavingsType.AutoSave &&
                    c.SavingStatus == Core.Enums.SavingStatus.Ongoing).ToListAsync();
                if (activeAutoSave == null)
                {
                    return ResultResponse.Failure("YOu don't hav any active 'Auto Save' Savings");
                }

                return ResultResponse.Success(activeAutoSave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}