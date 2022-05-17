using Dot.Application.ResponseModel;
using Dot.Infrastructure.Application.SchoolCommand.Command;
using Dot.Infrastructure.Application.SchoolCommand.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ApiController
    {
        [HttpPost("registerschool")]
        public async Task<ActionResult<ResultResponse>> RegisterSchool(CreateSchoolCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet("getallschools")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultResponse>> GetAllSchools()
        {
            return await Mediator.Send(new GetAllSchoolsQuery());
        }
    }
}
