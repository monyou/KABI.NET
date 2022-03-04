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
    public class ChangeUserPasswordByAdminCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }

        public class ChangeUserPasswordByAdminCommandHandler : IRequestHandler<ChangeUserPasswordByAdminCommand, Result>
        {
            private readonly IUserService userService;
            ILogger<ChangeUserPasswordByAdminCommandHandler> logger;

            public ChangeUserPasswordByAdminCommandHandler(
                IUserService userService,
                ILogger<ChangeUserPasswordByAdminCommandHandler> logger)
            {
                this.userService = userService;
                this.logger = logger;
            }

            public async Task<Result> Handle(ChangeUserPasswordByAdminCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        var user = await this.userService.GetUserByEmailAsync(request.Email);
                        user.ChangePassword(request.NewPassword);
                        await this.userService.UpdateUserAsync(user);

                        this.logger.LogInformation("Successfully changed password for user with email {email}.", request.Email);
                        return Result.Ok();
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in ChangeUserPasswordByAdminCommand: User is not with Admin role!");
                        return Result.Fail("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in ChangeUserPasswordByAdminCommand!");
                    return Result.Fail("Internal Server Error!");
                }
            }
        }
    }
}
