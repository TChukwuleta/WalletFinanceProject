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
    public class MerchantController : ApiController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string accessToken;
        public MerchantController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        }

        [HttpGet("getmerchantbyid/{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ResultResponse> GetMerchantById(string userid)
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, userid);
                return await Mediator.Send(new GetMerchantByIdQuery { UserId = userid });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("getstorespermerchant/{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ResultResponse> GetStoresPerMerchant(string userid)
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, userid);
                return await Mediator.Send(new GetMerchantChildrenByParentQuery { UserId = userid });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("gettransactionbymerchant/{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ResultResponse> GetTransactionByMerchant(string userid)
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, userid);
                return await Mediator.Send(new GetTransactionsByIdQuery { UserId = userid });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("getledgerpermerchant/{skip}/{take}/{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ResultResponse> GetLedgerPerMerchant(int skip, int take, string userid)
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, userid);
                return await Mediator.Send(new GetTransactionLedgerPerMerchantQuery { UserId = userid, Skip = skip, Take = take });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
