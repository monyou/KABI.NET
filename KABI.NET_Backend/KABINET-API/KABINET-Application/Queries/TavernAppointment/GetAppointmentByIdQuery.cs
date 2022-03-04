using KABINET_Application.Boundaries.TavernAppointment;
using KABINET_Application.Helpers;
using KABINET_Application.ViewModels;
using KABINET_Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KABINET_Application.Queries.TavernAppointment
{
    public class GetAppointmentByIdQuery : IRequest<Result<KABINET_Domain.Entities.TavernAppointment>>
    {
        public string Token { get; set; }
        public Guid TavernAppointmentId { get; set; }

        public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, Result<KABINET_Domain.Entities.TavernAppointment>>
        {
            private readonly ITavernAppointmentService tavernAppointmentService;
            private readonly ILogger<GetAppointmentByIdQueryHandler> logger;

            public GetAppointmentByIdQueryHandler(
                ITavernAppointmentService tavernAppointmentService,
                ILogger<GetAppointmentByIdQueryHandler> logger)
            {
                this.tavernAppointmentService = tavernAppointmentService;
                this.logger = logger;
            }

            public async Task<Result<KABINET_Domain.Entities.TavernAppointment>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        var appointment = await this.tavernAppointmentService.GetAppointmentById(request.TavernAppointmentId);

                        this.logger.LogInformation("Successfully fetched tavern appointment {@appointment}.", appointment);
                        return Result.Ok(appointment);
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in GetAppointmentByIdQueryHandler: User is not with Admin role!!");
                        return Result.Fail<KABINET_Domain.Entities.TavernAppointment>("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in GetAppointmentByIdQueryHandler!");
                    return Result.Fail<KABINET_Domain.Entities.TavernAppointment>("Internal Server Error!");
                }
            }
        }
    }
}
