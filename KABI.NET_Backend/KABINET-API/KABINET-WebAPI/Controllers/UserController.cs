using System.Threading.Tasks;
using KABINET_Application.Boundaries.Http;
using KABINET_Application.Boundaries.Logging;
using KABINET_Application.Commands.User;
using KABINET_Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace KABINET_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IHttpContextService httpContextService;
        private readonly ICorrelationIdService correlationIdService;

        public UserController(
             IMediator mediator,
             IHttpContextService httpContextService,
             ICorrelationIdService correlationIdService)
        {
            this.mediator = mediator;
            this.httpContextService = httpContextService;
            this.correlationIdService = correlationIdService;
        }

        [HttpGet("AllUsers")]
        public async Task<IActionResult> AllUsers()
        {
            var request = new GetAllUsersQuery()
            {
                Token = this.httpContextService.GetTokenFromHttpContext(HttpContext)
            };

            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await this.mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommand request)
        {
            request.Token = this.httpContextService.GetTokenFromHttpContext(HttpContext);

            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await this.mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok($"User with email {request.Email} was successfully deleted!");
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }
        }

        [HttpPut("ChangePasswordByAdmin")]
        public async Task<IActionResult> ChangePasswordByAdmin([FromBody] ChangeUserPasswordByAdminCommand request)
        {
            request.Token = this.httpContextService.GetTokenFromHttpContext(HttpContext);

            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await this.mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok($"Password of user with email {request.Email} is changed successfully!");
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }
        }

        [HttpPost("AddWarning")]
        public async Task<IActionResult> AddWarning([FromBody] AddWarningCommand request)
        {
            request.Token = this.httpContextService.GetTokenFromHttpContext(HttpContext);

            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await this.mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok($"User was warned successfully!");
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }
        }
    }
}