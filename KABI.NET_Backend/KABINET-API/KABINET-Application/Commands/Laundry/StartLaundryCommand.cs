using KABINET_Application.Boundaries.Laundry;
using KABINET_Application.Boundaries.User;
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
    public class StartLaundryCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Token { get; set; }

        public class StartLaundryCommandHandler : IRequestHandler<StartLaundryCommand, Result>
        {
            private readonly ILaundryService laundryService;
            private readonly IUserService userService;
            private readonly ILogger<StartLaundryCommandHandler> logger;

            public StartLaundryCommandHandler(
                ILaundryService laundryService,
                IUserService userService,
                ILogger<StartLaundryCommandHandler> logger)
            {
                this.laundryService = laundryService;
                this.userService = userService;
                this.logger = logger;
            }

            public async Task<Result> Handle(StartLaundryCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        var user = await this.userService.GetUserByEmailAsync(request.Email);

                        if (user != null)
                        {
                            user.AddLaundry(new KABINET_Domain.Entities.Laundry(user.Id));
                            await this.laundryService.StartLaundryAsync(user);

                            this.logger.LogInformation("Successfully started laundry session for user with email {email}.", request.Email);
                            return Result.Ok();
                        }
                        else
                        {
                            this.logger.LogWarning("Error occurred in StartLaundryCommandHandler: User with email {email} was not found!", request.Email);
                            return Result.Fail("Unable to access user data!");
                        }
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in StartLaundryCommandHandler: User is not with Admin role!!");
                        return Result.Fail("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in StartLaundryCommandHandler!");
                    return Result.Fail("Internal Server Error!");
                }
            }
        }
    }
}
