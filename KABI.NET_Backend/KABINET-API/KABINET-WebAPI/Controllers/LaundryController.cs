using System.Threading.Tasks;
using KABINET_Application.Boundaries.Http;
using KABINET_Application.Boundaries.Logging;
using KABINET_Application.Commands.Laundry;
using KABINET_Application.Queries.Laundry;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace KABINET_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LaundryController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IHttpContextService httpContextService;
        private readonly ICorrelationIdService correlationIdService;

        public LaundryController(
            IMediator mediator,
            IHttpContextService httpContextService,
            ICorrelationIdService correlationIdService)
        {
            this.mediator = mediator;
            this.httpContextService = httpContextService;
            this.correlationIdService = correlationIdService;
        }

        [HttpPost("Start")]
        public async Task<IActionResult> Start([FromBody] StartLaundryCommand request)
        {
            request.Token = this.httpContextService.GetTokenFromHttpContext(HttpContext);

            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await this.mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok("Landry begin successfully!");
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }
        }

        [HttpPost("Stop")]
        public async Task<IActionResult> Stop()
        {
            var request = new StopLaundryCommand()
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

        [HttpGet("GetLastRecord")]
        public async Task<IActionResult> GetLastRecord()
        {
            var request = new GetLaundryLastRecordQuery()
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

        [HttpPost("Pay")]
        public async Task<IActionResult> Pay([FromBody] PayLaundryCommand request)
        {
            request.Token = this.httpContextService.GetTokenFromHttpContext(HttpContext);

            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await this.mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok("Laundry is paid successfully!");
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }
        }
    }
}