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

namespace Dot.Application.UserCommand.LoginUser
{
    public class LoginUserCommand : IRequest<ResultResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ResultResponse>
    {
        private readonly IAuthService _authService;
        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ResultResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.loginAsync(request.Email, request.Password);
                return ResultResponse.Success(result);
            }
            catch (Exception ex)
            {

                return ResultResponse.Failure("User login failed");
            }
        }
    }
}
