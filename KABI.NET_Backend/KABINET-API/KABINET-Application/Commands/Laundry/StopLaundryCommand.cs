using KABINET_Application.Boundaries.Laundry;
using KABINET_Application.Helpers;
using KABINET_Application.ViewModels;
using KABINET_Application.ViewModels.Laundry;
using KABINET_Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KABINET_Application.Commands.Laundry
{
    public class StopLaundryCommand : IRequest<Result<LaundryVm>>
    {
        public string Token { get; set; }

        public class StopLaundryCommandHandler : IRequestHandler<StopLaundryCommand, Result<LaundryVm>>
        {
            private readonly ILaundryService laundryService;
            private readonly ILogger<StopLaundryCommandHandler> logger;
            private const double laundryTax = 1d;

            public StopLaundryCommandHandler(
                ILaundryService laundryService,
                ILogger<StopLaundryCommandHandler> logger)
            {
                this.laundryService = laundryService;
                this.logger = logger;
            }

            public async Task<Result<LaundryVm>> Handle(StopLaundryCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {

                        var laundry = await this.laundryService.GetLastRecordAsync();
                        laundry.StopLaundry(
                            DateTime.UtcNow,
                            CalculateLaundryTimeSpan(laundry),
                            CalculateTotal(laundry.TotalLaundryTime));
                        await this.laundryService.UpdateLaundryAsync(laundry);

                        this.logger.LogInformation("Successfully finished laundry session for user with email {email}.", userFromToken.Email);
                        return Result.Ok(new LaundryVm()
                        {
                            Id = laundry.Id,
                            StartTime = TimeZoneInfo.ConvertTimeFromUtc(laundry.StartTime, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time")),
                            EndTime = laundry.EndTime != null ? TimeZoneInfo.ConvertTimeFromUtc(laundry.EndTime.GetValueOrDefault(), TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time")) : laundry.EndTime,
                            TotalToPay = laundry.TotalToPay,
                            TotalLaundryTime = laundry.TotalLaundryTime,
                            User = laundry.User
                        });
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in StopLaundryCommandHandler: User is not with Admin role!!");
                        return Result.Fail<LaundryVm>("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in StopLaundryCommandHandler!");
                    return Result.Fail<LaundryVm>("Internal Server Error!");
                }
            }

            private TimeSpan CalculateLaundryTimeSpan(KABINET_Domain.Entities.Laundry laundry)
            {
                var now = DateTime.UtcNow;
                var laundryTimeSpan = now - laundry.StartTime;

                this.logger.LogInformation("Successfully calculate laundry TimeSpan {@laundryTimeSpan}.", laundryTimeSpan);
                return laundryTimeSpan;
            }

            private double CalculateTotal(TimeSpan laundryTime)
            {
                var totalToPay = Math.Round((laundryTime.TotalHours) * laundryTax);

                this.logger.LogInformation("Successfully calculated total to pay {totalToPay}.", totalToPay);
                return totalToPay;
            }
        }
    }
}
