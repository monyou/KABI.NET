using System.Threading.Tasks;
using KABINET_Application.Boundaries.Http;
using KABINET_Application.Boundaries.Logging;
using KABINET_Application.Commands.Laundry;
using KABINET_Application.Commands.TavernAppointment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace KABINET_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IHttpContextService httpContextService;
        private readonly ICorrelationIdService correlationIdService;

        public ReportsController(
            IMediator mediator,
            IHttpContextService httpContextService,
            ICorrelationIdService correlationIdService)
        {
            this.mediator = mediator;
            this.httpContextService = httpContextService;
            this.correlationIdService = correlationIdService;
        }

        [HttpGet("FullLaundryReport")]
        public async Task<IActionResult> FullLaundryReport()
        {
            var request = new SendLaundryFullReportCommand()
            {
                Token = this.httpContextService.GetTokenFromHttpContext(HttpContext)
            };

            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await this.mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }
        }

        [HttpGet("FullTavernAppointmentReport")]
        public async Task<IActionResult> FullTavernAppointmentReport()
        {
            var request = new SendTavernAppointmentFullReportCommand()
            {
                Token = this.httpContextService.GetTokenFromHttpContext(HttpContext)
            };

            var correlationId = this.correlationIdService.Get();

            using (LogContext.PushProperty("correlationId", correlationId))
            {
                var result = await this.mediator.Send(request);

                if (result.IsSuccess)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Error);
                }
            }
        }
    }
}