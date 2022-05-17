using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Application.UserCommand.ForgotPassword
{
    public class CreateForgotPasswordCommand : IRequest<ResultResponse>
    {
        public string Email { get; set; }
    }

    public class CreateForgotPasswordCommandHandler : IRequestHandler<CreateForgotPasswordCommand, ResultResponse>
    {
        private readonly IAuthService _authService;
        public CreateForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ResultResponse> Handle(CreateForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.Email is null)
                {
                    return ResultResponse.Failure("Invalid email");
                }

                var findUser = await _authService.GetUserByEmail(request.Email);
                if (!findUser.Succeeded)
                {
                    ResultResponse.Failure("Student cannot be found");
                }

                var generateOtp = await _authService.GenerateOTP(request.Email);
                if (!generateOtp.Succeeded)
                {
                    return ResultResponse.Failure("An error occured when generating OTP");
                }
                return ResultResponse.Success("OTP generated successfully. Kindly check your mail");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
