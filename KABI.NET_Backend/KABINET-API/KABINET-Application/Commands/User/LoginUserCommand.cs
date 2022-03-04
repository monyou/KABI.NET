using KABINET_Application.Boundaries.User;
using KABINET_Application.Helpers;
using KABINET_Application.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KABINET_Application.Commands.User
{
    public class LoginUserCommand : IRequest<Result<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string>>
        {
            private readonly IUserService userService;
            private readonly ILogger<LoginUserCommandHandler> logger;

            public LoginUserCommandHandler(
                 IUserService userService,
                 ILogger<LoginUserCommandHandler> logger)
            {
                this.userService = userService;
                this.logger = logger;
            }

            public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await this.userService.GetUserByEmailAsync(request.Email);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (user == null)
                    {
                        this.logger.LogWarning("Error occurred in LoginUserCommandHandler: User with email {email} was not found!", request.Email);
                        return Result.Fail<string>("No user found!");
                    }

                    bool status = this.userService.IsPasswordCorrect(request.Password, user.Password);

                    if (status)
                    {
                        this.logger.LogInformation("Successfully logged in user with email {email}.", request.Email);
                        return Result.Ok(JwtTokenHelper.ConvertToToken(user));
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in LoginUserCommandHandler: Invalid credentials!");
                        return Result.Fail<string>("Invalid credentials!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in LoginUserCommandHandler!");
                    return Result.Fail<string>("Internal Server Error!");
                }
            }
        }
    }
}
