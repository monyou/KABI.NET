using KABINET_Application.Boundaries.User;
using KABINET_Application.Helpers;
using KABINET_Application.ViewModels;
using KABINET_Application.ViewModels.User;
using KABINET_Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KABINET_Application.Queries.User
{
    public class GetAllUsersQuery : IRequest<Result<List<UserVm>>>
    {
        public string Token { get; set; }

        public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserVm>>>
        {
            private readonly IUserService userService;
            private readonly ILogger<GetAllUsersQueryHandler> logger;

            public GetAllUsersQueryHandler(
                IUserService userService,
                ILogger<GetAllUsersQueryHandler> logger)
            {
                this.userService = userService;
                this.logger = logger;
            }

            public async Task<Result<List<UserVm>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var allUserVms = new List<UserVm>();
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        var users = await this.userService.GetAllUsersAsync();
                        foreach (var user in users)
                        {
                            allUserVms.Add(new UserVm(
                                user.Id,
                                user.Email,
                                user.FirstName,
                                user.LastName,
                                user.Room,
                                user.Warnings,
                                user.Role));
                        }

                        this.logger.LogInformation("Successfully fetched all users {@allUserVms}.", allUserVms);
                        return Result.Ok(allUserVms);
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in GetAllUsersQueryHandler: User is not with Admin role!");
                        return Result.Fail<List<UserVm>>("You do not have access to this information!");
                    }

                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in GetAllUsersQueryHandler!");
                    return Result.Fail<List<UserVm>>("Internal Server Error!");
                }
            }
        }
    }
}
