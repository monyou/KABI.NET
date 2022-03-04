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
    public class DeleteUserCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Token { get; set; }

        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
        {
            private readonly IUserService userService;
            private readonly ILogger<DeleteUserCommandHandler> logger;

            public DeleteUserCommandHandler(
                IUserService userService,
                ILogger<DeleteUserCommandHandler> logger)
            {
                this.userService = userService;
                this.logger = logger;
            }

            public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        await this.userService.DeleteUserByEmailAsync(request.Email);

                        this.logger.LogInformation("Successfully deleted users with email {email}.", request.Email);
                        return Result.Ok();
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in DeleteUserCommandHandler: User is not with Admin role!");
                        return Result.Fail("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in DeleteUserCommandHandler!");
                    return Result.Fail("Internal Server Error!");
                }
            }
        }
    }
}
