using Dot.Application.ResponseModel;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.Transaction.Queries
{
    public class GetTransactionsByIdQuery : IRequest<ResultResponse>
    {
        public string UserId { get; set; }
    }

    public class GetTransactionsByIdQueryHandler : IRequestHandler<GetTransactionsByIdQuery, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetTransactionsByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(GetTransactionsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userTransactions = await _context.Transactions.Where(c => c.UserId == request.UserId).ToListAsync();
                if(userTransactions == null)
                {
                    return ResultResponse.Failure("No transaction found for this user");
                }
                return  ResultResponse.Success(userTransactions);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
