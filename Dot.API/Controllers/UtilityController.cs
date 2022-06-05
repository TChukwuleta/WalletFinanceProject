using Dot.Application.ResponseModel;
using Dot.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        [HttpGet("getcompanysector")]
        public async Task<ActionResult<ResultResponse>> GetCompanySector()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((ComapnySector[])Enum.GetValues(typeof(ComapnySector))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get Company sector enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getcurrencycode")]
        public async Task<ActionResult<ResultResponse>> GetCurrencyCode()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((CurrencyCode[])Enum.GetValues(typeof(CurrencyCode))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get currency code enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getfundingsource")]
        public async Task<ActionResult<ResultResponse>> GetFundingSource()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((FundingSource[])Enum.GetValues(typeof(FundingSource))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get Funding source enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getgender")]
        public async Task<ActionResult<ResultResponse>> GetGender()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((Gender[])Enum.GetValues(typeof(Gender))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get gender enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getpaymenttype")]
        public async Task<ActionResult<ResultResponse>> GetPaymentType()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((MerchantPaymentType[])Enum.GetValues(typeof(MerchantPaymentType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get Payment Type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsavingfrequency")]
        public async Task<ActionResult<ResultResponse>> GetSavingFrequency()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((SavingFrequency[])Enum.GetValues(typeof(SavingFrequency))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get Saving Frequency enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsavingstatus")]
        public async Task<ActionResult<ResultResponse>> GetSavingStatus()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((SavingStatus[])Enum.GetValues(typeof(SavingStatus))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get saving status enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getsavingstype")]
        public async Task<ActionResult<ResultResponse>> GetSavingsType()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((SavingsType[])Enum.GetValues(typeof(SavingsType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get Savings type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getstatus")]
        public async Task<ActionResult<ResultResponse>> GetStatus()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((Status[])Enum.GetValues(typeof(Status))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get status enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("gettransactionstatus")]
        public async Task<ActionResult<ResultResponse>> GetTransactionStatus()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((TransactionStatus[])Enum.GetValues(typeof(TransactionStatus))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get transaction status enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("gettransactiontype")]
        public async Task<ActionResult<ResultResponse>> GetTransactionType()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((TransactionType[])Enum.GetValues(typeof(TransactionType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get transaction type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }

        [HttpGet("getusertype")]
        public async Task<ActionResult<ResultResponse>> GetUserType()
        {
            try
            {
                return await Task.Run(() => ResultResponse.Success(
                 ((UserType[])Enum.GetValues(typeof(UserType))).Select(x => new { Value = (int)x, Name = x.ToString() }).ToList()
                 ));
            }
            catch (Exception ex)
            {
                return ResultResponse.Failure(new string[] { "Get user type enums failed" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
