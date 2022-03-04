using System.Threading.Tasks;
using KABINET_Application.Boundaries.Http;
using KABINET_Application.Boundaries.Logging;
using KABINET_Application.Commands.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace KABINET_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IHttpContextService httpContextService;
        private readonly ICorrelationIdService correlationIdService;

        public AccountController(
            IMediator mediator,
            IHttpContextService httpContextService,
            ICorrelationIdService correlationIdService)
        {
            this.mediator = mediator;
            this.httpContextService = httpContextService;
            this.correlationIdService = correlationIdService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
        {
            request.Token = this.httpContextService.GetTokenFromHttpContext(HttpContext);

            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await mediator.Send(request);

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

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
        {
            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await mediator.Send(request);

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

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand request)
        {
            request.Token = this.httpContextService.GetTokenFromHttpContext(HttpContext);

            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await this.mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok("Password changed successfully!");
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }
        }
    }
}