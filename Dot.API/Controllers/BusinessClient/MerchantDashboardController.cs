using Dot.Application.ResponseModel;
using Dot.Infrastructure.Application.MerchantSide.Queries;
using Dot.Infrastructure.Application.Transaction.Queries;
using Dot.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dot.API.Controllers.BusinessClient
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantDashboardController : ApiController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string accessToken;
        public MerchantDashboardController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        }

        /*[HttpGet("getbalanceandspendcard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<ResultResponse>> GetBalanceAndSpendCard()
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, userid);
                return await Mediator.Send(new GetAllSavingsByUserQuery { UserId = userid });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }*/

        [HttpGet("getmonthlytransactioncountandvaluebymerchant/{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ResultResponse> GetMonthlyTransactionCountAndValueByMerchant(string userid)
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, userid);
                return await Mediator.Send(new GetTransactionCountValuePerMerchantQuery { UserId = userid });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
