using Dot.Application.ResponseModel;
using Dot.Core.Enums;
using Dot.Infrastructure.Application.Transaction.MerchantSide.Queries;
using Dot.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dot.API.Controllers.BusinessClient
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantStoresDashboardController : ApiController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string accessToken;
        public MerchantStoresDashboardController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        }

        [HttpGet("getstoredashboardcount/{merchantuserid}/{storeuserid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ResultResponse> GetStoreDashboardCount(string merchantuserid, string storeuserid)
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, merchantuserid);
                return await Mediator.Send(new GetChildMerchantDashboardQuery { MerchantUserId = merchantuserid, StoreUserId = storeuserid });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("getstoredashboardbypamenttype/{merchantuserid}/{storeuserid}/{paymenttype}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ResultResponse> GetStoreDashboardByPaymentType(string merchantuserid, string storeuserid, int paymenttype)
        {
            try
            {
                var userToken = Token.ExtractToken(accessToken);
                Token.ValidateToken(userToken, merchantuserid);
                return await Mediator.Send(new GetChildMerchantDashboartByPaymentTypeQuery { UserId = storeuserid, PaymentType = (MerchantPaymentType)paymenttype });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
