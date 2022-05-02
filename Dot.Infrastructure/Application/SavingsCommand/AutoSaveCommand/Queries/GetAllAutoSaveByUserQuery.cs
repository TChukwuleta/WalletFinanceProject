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
    public class GetAllAutoSaveByUserQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GetAllAutoSaveByUserQueryHandler : IRequestHandler<GetAllAutoSaveByUserQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetAllAutoSaveByUserQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetAllAutoSaveByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var autoSave = await _context.Savings.Where(
                    c => c.UserId == request.UserId &&
                    c.StudentId == request.StudentId
                    && c.SavingsType == Core.Enums.SavingsType.AutoSave).ToListAsync();
                if (autoSave == null)
                {
                    return ResultResponse.Failure("YOu don't hav any 'Auto Save' Savings");
                }

                return ResultResponse.Success(autoSave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}