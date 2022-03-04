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
    public class RegisterUserCommand : IRequest<Result<Guid>>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Room { get; set; }
        public string Token { get; set; }

        public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
        {
            private readonly IUserService userService;
            private readonly ILogger<RegisterUserCommandHandler> logger;

            public RegisterUserCommandHandler(
                IUserService userService,
                ILogger<RegisterUserCommandHandler> logger)
            {
                this.userService = userService;
                this.logger = logger;
            }

            public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        var user = await this.userService.GetUserByEmailAsync(request.Email);

                        if (user == null)
                        {
                            var newUserId = await this.userService.CreateUserAsync(request);

                            this.logger.LogInformation("Successfully registered users with email {email}.", request.Email);
                            return Result.Ok(newUserId);
                        }

                        this.logger.LogWarning("Error occurred in RegisterUserCommandHandler: User with email {email} already exists in our database!", request.Email);
                        return Result.Fail<Guid>("The user already exist in our database!");
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in RegisterUserCommandHandler: User is not with Admin role!");
                        return Result.Fail<Guid>("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in RegisterUserCommandHandler!");
                    return Result.Fail<Guid>("Internal Server Error!");
                }
            }
        }
    }
}
