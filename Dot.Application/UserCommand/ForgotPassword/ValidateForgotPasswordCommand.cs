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
    public class ValidateForgotPasswordCommand : IRequest<ResultResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string otp { get; set; }
    }

    public class ValidateForgotPasswordCommandHandler : IRequestHandler<ValidateForgotPasswordCommand, ResultResponse>
    {
        private readonly IAuthService _authService;

        public ValidateForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ResultResponse> Handle(ValidateForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Email is null)
                {
                    return ResultResponse.Failure("Invalid email");
                }

                var findUser = await _authService.GetUserByEmail(request.Email);
                if (!findUser.Succeeded)
                {
                    ResultResponse.Failure("Student cannot be found");
                }

                var validateOtp = await _authService.validateOTP(request.Email, request.otp, request.Password);
                if (!validateOtp)
                {
                    return ResultResponse.Failure("OTP Validation and password reset failed");
                }
                return ResultResponse.Success("Your password reset was successful");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
