using Dot.Application.ResponseModel;
using Dot.Infrastructure.Application.SavingsCommand;
using Dot.Infrastructure.Application.SavingsCommand.AutoSaveCommand.Command;
using Dot.Infrastructure.Application.SavingsCommand.AutoSaveCommand.Queries;
using Dot.Infrastructure.Application.SavingsCommand.GroupSavingsCommand.Command;
using Dot.Infrastructure.Application.SavingsCommand.GroupSavingsCommand.Queries;
using Dot.Infrastructure.Application.SavingsCommand.LockAndSaveCommand;
using Dot.Infrastructure.Application.SavingsCommand.LockAndSaveCommand.Queries;
using Dot.Infrastructure.Application.SavingsCommand.SaveAsYouGoCommand.Command;
using Dot.Infrastructure.Application.SavingsCommand.SaveAsYouGoCommand.Queries;
using Dot.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingsController : ApiController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string accessToken;

        public SavingsController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            accessToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        }

        // Group Savings
        [HttpPost("creategroupsaving")]
        public async Task<ActionResult<ResultResponse>> CreateGroupSaving(CreateGroupSavingsCommand command)
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

        [HttpPost("updategroupsaving")]
        public async Task<ActionResult<ResultResponse>> UpdateGroupSaving(UpdateGroupSavingCommand command)
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

        [HttpGet("getactivegroupsaving/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetActiveGroupSaving(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetAllActiveGroupSavingByUserQuery { UserId = userid, StudentId = studentid });
        }


        [HttpGet("getgroupsaving/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetGroupSaving(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetAllGroupSavingByUserQuery { UserId = userid, StudentId = studentid });
        }



        // Auto Save
        [HttpPost("createautosave")]
        public async Task<ActionResult<ResultResponse>> CreateAutoSave(CreateAutoSaveCommand command)
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

        [HttpPost("updateautosave")]
        public async Task<ActionResult<ResultResponse>> UpdateAutoSabe(UpdateAutoSaveCommand command)
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

        [HttpGet("getactiveautosave/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetActiveAutoSave(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetAllActiveAutoSaveQuery { UserId = userid, StudentId = studentid });
        }


        [HttpGet("getautosave/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetAutoSave(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetAllAutoSaveByUserQuery { UserId = userid, StudentId = studentid });
        }



        // Save As You Go
        [HttpPost("createsaveasyougo")]
        public async Task<ActionResult<ResultResponse>> CreateSaveAsYouGo(CreateSaveAsYouGoCommand command)
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

        [HttpPost("updatesaveasyougo")]
        public async Task<ActionResult<ResultResponse>> UpdateSaveAsYouGo(UpdateSaveAsYouGoCommand command)
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

        [HttpGet("getactivesaveasyougo/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetActiveSaveAsYouGo(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetAllActiveSaveAsYouGoByUserQuery { UserId = userid, StudentId = studentid });
        }


        [HttpGet("getsaveasyougo/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetSaveAsYouGo(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetAllSaveAsYouGoByUserQuery { UserId = userid, StudentId = studentid });
        }


        // Lock And Save
        [HttpPost("createlockandsave")]
        public async Task<ActionResult<ResultResponse>> CreateLockAndSave(CreateLockAndSaveCommand command)
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

        [HttpPost("updatelockandsave")]
        public async Task<ActionResult<ResultResponse>> UpdateLockAndSave(UpdateSaveAndLockCommand command)
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

        [HttpPost("extendlockandsave")]
        public async Task<ActionResult<ResultResponse>> ExtendLockAndSave(ExtendLockAndSaveCommand command)
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

        [HttpGet("getactivelockandsave/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetActiveLockAndSave(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetAllActiveLockAndSaveByUserQuery { UserId = userid, StudentId = studentid });
        }


        [HttpGet("getlockandsave/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetLockAndSave(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetAllLockAndSaveByUserQuery { UserId = userid, StudentId = studentid });
        }


        // Get ALl User Savings Product
        [HttpGet("getallsavings/{userid}/{studentid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetAllSavings(string userid, string studentid)
        {
            var userToken = Token.ExtractToken(accessToken);
            Token.ValidateToken(userToken, userid);
            return await Mediator.Send(new GetAllSavingsByUserQuery { UserId = userid, StudentId = studentid });
        }
    }
}
