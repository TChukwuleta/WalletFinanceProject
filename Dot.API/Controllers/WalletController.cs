using Dot.Application.ResponseModel;
using Dot.Infrastructure.Application.WalletCommand;
using Dot.Infrastructure.Application.WalletQueries;
using Dot.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ApiController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string accessToken;

        public WalletController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        }
        [HttpPost("createwallet")]
        public async Task<ActionResult<ResultResponse>> CreateWallet(CreateWalletCommand command)
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

        [HttpPost("wallettransfer")]
        public async Task<ActionResult<ResultResponse>> WalletTransfer(WalletTransferCommand command)
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

        [HttpGet("getwalletbalance/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetWalletBalance(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetWalletBalance { UserId = userid });
        }

        [HttpGet("getwalletdetails/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetWalletDetails(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetWalletDetails { UserId = userid });
        }
    }
}
