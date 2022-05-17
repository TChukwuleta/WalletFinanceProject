using Dot.Application.ResponseModel;
using Dot.Infrastructure.Application.CardCommand.Command;
using Dot.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ApiController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string accessToken;

        public CardController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        }


        [HttpPost("createcard")]
        public async Task<ActionResult<ResultResponse>> CreateCard(CreateCardCommand command)
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, command.UserId);
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
