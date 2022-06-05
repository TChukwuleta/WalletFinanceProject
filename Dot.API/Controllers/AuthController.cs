using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using Dot.Application.UserCommand.ChangeUserPassword;
using Dot.Application.UserCommand.CreateUser;
using Dot.Application.UserCommand.ForgotPassword;
using Dot.Application.UserCommand.LoginUser;
using Dot.Infrastructure.Application.MerchantSide.Authentication.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registerstudent")]
        public async Task<ActionResult<ResultResponse>> RegisterUser(CreateStudentCommand command) 
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost("registermerchant")]
        public async Task<ActionResult<ResultResponse>> RegisterMerchant(CreateClientCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost("loginuser")]
        public async Task<ActionResult<ResultResponse>> LoginUser(LoginUserCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("resetpassword")]
        public async Task<ActionResult<ResultResponse>> ResetPassword(ChangeUserPasswordCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                throw; 
            }
        }


        [HttpPost("forgotpassword")]
        public async Task<ActionResult<ResultResponse>> ForgotPassword(CreateForgotPasswordCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("forgotpasswordvalidation")]
        public async Task<ActionResult<ResultResponse>> ForgotPasswordValidation(ValidateForgotPasswordCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /*[HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<Employee.Core.Entities.Employee>> Get()
        {
            return await _mediator.Send(new GetAllEmployeeQuery());
        }*/
    }
}
