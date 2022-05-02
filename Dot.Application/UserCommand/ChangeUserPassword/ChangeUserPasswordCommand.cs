using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Application.UserCommand.ChangeUserPassword
{
    public class ChangeUserPasswordCommand : IRequest<ResultResponse> 
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, ResultResponse>
    {
        private readonly IAuthService _authService;
        public ChangeUserPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<ResultResponse> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _authService.ChangePasswordAsync(request.Email, request.CurrentPassword, request.NewPassword);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
