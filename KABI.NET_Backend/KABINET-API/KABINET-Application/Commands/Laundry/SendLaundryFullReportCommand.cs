using KABINET_Application.Boundaries.Reports;
using KABINET_Application.Helpers;
using KABINET_Application.ViewModels;
using KABINET_Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KABINET_Application.Commands.Laundry
{
    public class SendLaundryFullReportCommand : IRequest<Result>
    {
        public string Token { get; set; }

        public class SendLaundryFullReportCommandHandler : IRequestHandler<SendLaundryFullReportCommand, Result>
        {
            private readonly IReportsService reportsService;
            private readonly ILogger<SendLaundryFullReportCommandHandler> logger;

            public SendLaundryFullReportCommandHandler(
                IReportsService reportsService,
                ILogger<SendLaundryFullReportCommandHandler> logger)
            {
                this.reportsService = reportsService;
                this.logger = logger;
            }

            public async Task<Result> Handle(SendLaundryFullReportCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        await this.reportsService.SendLaundryFullReportAsync();

                        this.logger.LogInformation("Successfully sent full laundry report.");
                        return Result.Ok();
                    }
                    else
                    {
                        this.logger.LogWarning($"Error occurred in SendLaundryFullReportCommandHandler: User is not with Admin role!");
                        return Result.Fail("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in SendLaundryFullReportCommandHandler!");
                    return Result.Fail("Internal Server Error!");
                }
            }
        }
    }
}
