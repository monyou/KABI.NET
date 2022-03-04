using KABINET_Application.Boundaries.Reports;
using KABINET_Application.Helpers;
using KABINET_Application.ViewModels;
using KABINET_Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KABINET_Application.Commands.TavernAppointment
{
    public class SendTavernAppointmentFullReportCommand : IRequest<Result>
    {
        public string Token { get; set; }

        public class SendTavernAppointmentFullReportCommandHandler : IRequestHandler<SendTavernAppointmentFullReportCommand, Result>
        {
            private readonly IReportsService reportsService;
            private readonly ILogger<SendTavernAppointmentFullReportCommandHandler> logger;

            public SendTavernAppointmentFullReportCommandHandler(
                IReportsService reportsService,
                ILogger<SendTavernAppointmentFullReportCommandHandler> logger)
            {
                this.reportsService = reportsService;
                this.logger = logger;
            }

            public async Task<Result> Handle(SendTavernAppointmentFullReportCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        await this.reportsService.SendTavernAppointmentFullReportAsync();

                        this.logger.LogInformation("Successfully sent tavern appointments full report.");
                        return Result.Ok();
                    }
                    else
                    {
                        this.logger.LogWarning($"Error occurred in SendTavernAppointmentFullReportCommandHandler: User is not with Admin role!");
                        return Result.Fail("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in SendTavernAppointmentFullReportCommandHandler!");
                    return Result.Fail("Internal Server Error!");
                }
            }
        }
    }
}
