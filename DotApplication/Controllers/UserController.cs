using Dot.Application.Interfaces;
using Dot.Application.ResponseModel;
using Dot.Application.UserCommand.ChangeUserPassword;
using Dot.Application.UserCommand.CreateUser;
using Dot.Application.UserCommand.LoginUser;
using Dot.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registeruser")]
        public async Task<ActionResult<ResultResponse>> RegisterUser(CreateUserCommand command)
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

        /*[HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<Employee.Core.Entities.Employee>> Get()
        {
            return await _mediator.Send(new GetAllEmployeeQuery());
        }*/
    }
}
