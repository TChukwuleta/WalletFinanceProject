using Dot.Application.ResponseModel;
using Dot.Infrastructure.Application.RequestCommand;
using Dot.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestMoneyController : ApiController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string accessToken;

        public RequestMoneyController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        }

        [HttpPost("requestformoney")]
        public async Task<ActionResult<ResultResponse>> WalletTransfer(CreateRequestCommand command)
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, command.UserId);
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
