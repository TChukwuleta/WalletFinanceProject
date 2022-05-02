using Dot.Application.ResponseModel;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.SavingsCommand.SaveAsYouGoCommand.Queries
{
    public class GetAllActiveSaveAsYouGoByUserQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
        public string StudentId { get; set; }
    }

    public class GetAllActiveSaveAsYouGoByUserQueryHandler : IRequestHandler<GetAllActiveSaveAsYouGoByUserQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetAllActiveSaveAsYouGoByUserQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetAllActiveSaveAsYouGoByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var findUser = await _context.Students.FirstOrDefaultAsync(c => c.UserId == request.UserId && c.Status == Core.Enums.Status.Active);
                if (findUser == null)
                {
                    return ResultResponse.Failure("Invalid active user");
                }

                var activeSaveAsYouGo = await _context.Savings.Where(
                    c => c.UserId == request.UserId &&
                    c.StudentId == request.StudentId
                    && c.SavingsType == Core.Enums.SavingsType.SaveAsYouGo &&
                    c.SavingStatus == Core.Enums.SavingStatus.Ongoing).ToListAsync();
                if (activeSaveAsYouGo == null)
                {
                    return ResultResponse.Failure("YOu don't hav any 'Save as you go' Savings");
                }

                return ResultResponse.Success(activeSaveAsYouGo);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}