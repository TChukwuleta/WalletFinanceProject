using Dot.Application.ResponseModel;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.SchoolCommand.Queries
{
    public class GetAllSchoolsQuery : IRequest<ResultResponse>
    {
    }

    public class GetAllSchoolsQueryHandler : IRequestHandler<GetAllSchoolsQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetAllSchoolsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetAllSchoolsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allSchools = await _context.Schools.ToListAsync();
                if(allSchools.Count <= 0)
                {
                    return ResultResponse.Failure("No School found");
                }

                return ResultResponse.Success(allSchools);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
