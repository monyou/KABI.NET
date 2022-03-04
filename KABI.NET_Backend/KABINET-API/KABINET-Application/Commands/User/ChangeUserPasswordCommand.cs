using KABINET_Application.Boundaries.User;
using KABINET_Application.Helpers;
using KABINET_Application.ViewModels;
using KABINET_Common;
using KABINET_Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KABINET_Application.Commands.User
{
    public class ChangeUserPasswordCommand : IRequest<Result>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }

        public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, Result>
        {
            private readonly IUserService userService;
            private readonly ILogger<ChangeUserPasswordCommandHandler> logger;

            public ChangeUserPasswordCommandHandler(
                IUserService userService,
                ILogger<ChangeUserPasswordCommandHandler> logger)
            {
                this.userService = userService;
                this.logger = logger;
            }

            public async Task<Result> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Member || userFromToken.Role == UserRoles.Admin)
                    {
                        var user = await this.userService.GetUserByEmailAsync(userFromToken.Email);

                        string oldPasswordHashed = HashUtils.Hash(request.OldPassword);

                        if (HashUtils.Verify(request.OldPassword, user.Password))
                        {
                            string newPasswordHashed = HashUtils.Hash(request.NewPassword);
                            user.ChangePassword(newPasswordHashed);
                            await this.userService.UpdateUserAsync(user);

                            this.logger.LogInformation("Successfully changed password for user with email {email}.", userFromToken.Email);
                            return Result.Ok();
                        }
                    }
                    else if (userFromToken.Role == UserRoles.Blocked)
                    {
                        this.logger.LogWarning("Error occurred in ChangeUserPasswordCommandHandler: User with email {email} is blocked!", userFromToken.Email);
                        return Result.Fail("User is blocked!");
                    }

                    this.logger.LogWarning("Error occurred in ChangeUserPasswordCommandHandler: Wrong old password for user with email {email}!", userFromToken.Email);
                    return Result.Fail("Your account is invalid");
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in ChangeUserPasswordCommandHandler!");
                    return Result.Fail("Internal Server Error!");
                }
            }
        }
    }
}
