using KABINET_Application.Boundaries.Laundry;
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
    public class PayLaundryCommand : IRequest<Result>
    {
        public string LaundryId { get; set; }
        public string Token { get; set; }

        public class PayLaundryCommandHandler : IRequestHandler<PayLaundryCommand, Result>
        {
            private readonly ILaundryService laundryService;
            private readonly ILogger<PayLaundryCommandHandler> logger;

            public PayLaundryCommandHandler(
                ILaundryService laundryService,
                ILogger<PayLaundryCommandHandler> logger)
            {
                this.laundryService = laundryService;
                this.logger = logger;
            }

            public async Task<Result> Handle(PayLaundryCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        var laundry = await this.laundryService.GetLaundryByIdAsync(Guid.Parse(request.LaundryId));
                        laundry.PayLaundry();
                        await this.laundryService.UpdateLaundryAsync(laundry);

                        this.logger.LogInformation("Successfully paid for laundry with id {laundryId}.", request.LaundryId);
                        return Result.Ok();
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in PayLaundryCommandHandler: User is not with Admin role!!");
                        return Result.Fail("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in PayLaundryCommandHandler!");
                    return Result.Fail("Internal Server Error!");
                }
            }
        }
    }
}
