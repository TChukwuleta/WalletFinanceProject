using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Application.SchoolCommand.Command
{
    public class CreateSchoolCommand : IRequest<ResultResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
    }

    public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, ResultResponse>
    {
        private readonly ApplicationDbContext _context;
        public CreateSchoolCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var findSchool = await _context.Schools.FirstOrDefaultAsync(c => c.Email == request.Email);
                if(findSchool != null)
                {
                    return ResultResponse.Failure("School details already exist");
                }

                var newSchool = new School
                {
                    Name = request.Name,
                    Email = request.Email,
                    ContactNumber = request.ContactNumber,
                    Address = request.Address,
                    Country = request.Country
                };

                await _context.Schools.AddAsync(newSchool);
                await _context.SaveChangesAsync(cancellationToken);

                return ResultResponse.Success("School creation was successful", newSchool);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
