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

namespace KABINET_Application.Queries.Laundry
{
    public class GetLaundryLastRecordQuery : IRequest<Result<LaundryVm>>
    {
        public string Token { get; set; }

        public class GetLaundryLastRecordQueryHandler : IRequestHandler<GetLaundryLastRecordQuery, Result<LaundryVm>>
        {
            private readonly ILaundryService laundryService;
            private readonly ILogger<GetLaundryLastRecordQueryHandler> logger;

            public GetLaundryLastRecordQueryHandler(
                 ILaundryService laundryService,
                 ILogger<GetLaundryLastRecordQueryHandler> logger)
            {
                this.laundryService = laundryService;
                this.logger = logger;
            }

            public async Task<Result<LaundryVm>> Handle(GetLaundryLastRecordQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin || userFromToken.Role == UserRoles.Member)
                    {
                        var laundry = await this.laundryService.GetLastRecordAsync();

                        this.logger.LogInformation("Successfully fetched last laundry record {@laundry}.", laundry);
                        if (laundry != null)
                        {
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
                            return Result.Ok<LaundryVm>(null);
                        }
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in GetLaundryLastRecordQueryHandler: User with email {email} is blocked!!", userFromToken.Email);
                        return Result.Fail<LaundryVm>("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in GetLaundryLastRecordQueryHandler!");
                    return Result.Fail<LaundryVm>("Internal Server Error!");
                }
            }
        }
    }
}
