using System.Threading.Tasks;
using KABINET_Application.Boundaries.Http;
using KABINET_Application.Boundaries.Logging;
using KABINET_Application.Commands.TavernAppointment;
using KABINET_Application.Queries.TavernAppointment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace KABINET_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class TavernAppointmentController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IHttpContextService httpContextService;
        private readonly ICorrelationIdService correlationIdService;

        public TavernAppointmentController(
            IMediator mediator,
            IHttpContextService httpContextService,
            ICorrelationIdService correlationIdService)
        {
            this.mediator = mediator;
            this.httpContextService = httpContextService;
            this.correlationIdService = correlationIdService;
        }

        [HttpPost("AddAppointment")]
        public async Task<IActionResult> AddAppointment([FromBody] AddTavernAppointmentCommand request)
        {
            request.Token = this.httpContextService.GetTokenFromHttpContext(HttpContext);

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

        [HttpGet("AllAppointments")]
        public async Task<IActionResult> AllAppointments()
        {
            var request = new GetAllAppointmentsQuery()
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

        [HttpPost("AppointmentById")]
        public async Task<IActionResult> AppointmentById([FromBody] GetAppointmentByIdQuery request)
        {
            request.Token = this.httpContextService.GetTokenFromHttpContext(HttpContext);

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
    }
}
