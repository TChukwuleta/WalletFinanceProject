using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using Dot.Core.Entities;
using Dot.Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Application.UserCommand.CreateUser
{
    public class CreateStudentCommand : IRequest<ResultResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string StudentID { get; set; }
        public string Email { get; set; }
        public string SchoolName { get; set; }
        public Gender Gender { get; set; }
    }

    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, ResultResponse>
    {
        private readonly IAuthService _authService;
        public CreateStudentCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ResultResponse> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newUser = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = request.Password,
                    Address = request.Address,
                    StudentID = request.StudentID,
                    Email = request.Email,
                    Gender = request.Gender,
                    SchoolName = request.SchoolName
                };

                var result = await _authService.CreateUserAsync(newUser);
                if (!result.Succeeded)
                {
                    var errorMessage = "";
                    if (result.Messages != null)
                    {
                        if (result.Messages.Count() > 0)
                        {
                            foreach (var item in result.Messages)
                            {
                                errorMessage = item;
                            }
                            return ResultResponse.Failure($"Unable to create client. {errorMessage}");
                        }
                    }
                    return ResultResponse.Failure($"Unable to create client. {result.Message}");
                }
                return ResultResponse.Success("Student Account creation was successfully");
            }
            catch (Exception ex)
            {

                return ResultResponse.Failure("User creation failed");
            }
        }
    }
}
