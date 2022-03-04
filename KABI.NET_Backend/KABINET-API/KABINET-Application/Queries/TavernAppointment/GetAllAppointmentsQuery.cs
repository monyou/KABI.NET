using KABINET_Application.Boundaries.TavernAppointment;
using KABINET_Application.Helpers;
using KABINET_Application.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KABINET_Application.Queries.TavernAppointment
{
    public class GetAllAppointmentsQuery : IRequest<Result<List<KABINET_Domain.Entities.TavernAppointment>>>
    {
        public string Token { get; set; }

        public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, Result<List<KABINET_Domain.Entities.TavernAppointment>>>
        {
            private readonly ITavernAppointmentService tavernAppointmentService;
            private readonly ILogger<GetAllAppointmentsQueryHandler> logger;

            public GetAllAppointmentsQueryHandler(
                ITavernAppointmentService tavernAppointmentService,
                ILogger<GetAllAppointmentsQueryHandler> logger)
            {
                this.tavernAppointmentService = tavernAppointmentService;
                this.logger = logger;
            }

            public async Task<Result<List<KABINET_Domain.Entities.TavernAppointment>>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    var appointments = await this.tavernAppointmentService.GetAllAppointments();

                    this.logger.LogInformation("Successfully fetched all tavern appointments {@appointments}.", appointments);
                    return Result.Ok(appointments);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in GetAllAppointmentsQueryHandler!");
                    return Result.Fail<List<KABINET_Domain.Entities.TavernAppointment>>("Internal Server Error!");
                }
            }
        }
    }
}
