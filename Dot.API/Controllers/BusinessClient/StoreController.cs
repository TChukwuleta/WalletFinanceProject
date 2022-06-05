using Dot.Application.ResponseModel;
using Dot.Infrastructure.Application.Transaction.Queries;
using Dot.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dot.API.Controllers.BusinessClient
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ApiController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string accessToken;

        public StoreController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        }

        [HttpGet("gettransactionbystore/{merchantuserid}/{storeuserid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ResultResponse> GetTransactionByStore(string merchantuserid, string storeuserid)
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, merchantuserid);
                return await Mediator.Send(new GetTransactionsByIdQuery { UserId = storeuserid });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
