using KABINET_Application.Boundaries.User;
using KABINET_Application.Helpers;
using KABINET_Application.ViewModels;
using KABINET_Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KABINET_Application.Commands.User
{
    public class AddWarningCommand : IRequest<Result>
    {
        public string Token { get; set; }
        public string Email { get; set; }

        public class AddWarningCommandHandler : IRequestHandler<AddWarningCommand, Result>
        {
            private readonly IUserService userService;
            private readonly ILogger<AddWarningCommandHandler> logger;

            public AddWarningCommandHandler(
                IUserService userService,
                ILogger<AddWarningCommandHandler> logger)
            {
                this.userService = userService;
                this.logger = logger;
            }

            public async Task<Result> Handle(AddWarningCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        var user = await this.userService.GetUserByEmailAsync(request.Email);
                        user.AddWarning();
                        await this.userService.UpdateUserAsync(user);

                        this.logger.LogInformation("Successfully added warning to user with email {email}.", request.Email);
                        return Result.Ok();
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in AddWarningCommandHandler: User is not with Admin role!");
                        return Result.Fail("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in AddWarningCommandHandler!");
                    return Result.Fail("Internal Server Error!");
                }
            }
        }
    }
}
