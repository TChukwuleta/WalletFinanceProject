using Dot.Application.ResponseModel;
using Dot.Infrastructure.Application.BarcodeCommand.Commands;
using Dot.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarcodeController : ApiController
    {
        protected readonly IHttpContextAccessor _contextAccessor;
        private readonly string accessToken;

        public BarcodeController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        }


        [HttpPost("generatebarcode")]
        public async Task<ActionResult<ResultResponse>> GenerateBarcode(GenerateBarcodeCommand command)
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

        [HttpPost("scantopay")]
        public async Task<ActionResult<ResultResponse>> ScanToPay(ScanToPayCommand command)
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