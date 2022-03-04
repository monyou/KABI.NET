using KABINET_Application.Boundaries.TavernAppointment;
using KABINET_Application.Boundaries.User;
using KABINET_Application.Helpers;
using KABINET_Application.ViewModels;
using KABINET_Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KABINET_Application.Commands.TavernAppointment
{
    public class AddTavernAppointmentCommand : IRequest<Result<Guid>>
    {
        public string Token { get; set; }
        public string UserEmail { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; }

        public class AddTavernAppointmentCommandHandler : IRequestHandler<AddTavernAppointmentCommand, Result<Guid>>
        {
            private readonly ITavernAppointmentService tavernAppointmentService;
            private readonly IUserService userService;
            private readonly ILogger<AddTavernAppointmentCommandHandler> logger;

            public AddTavernAppointmentCommandHandler(
                ITavernAppointmentService tavernAppointmentService,
                IUserService userService,
                ILogger<AddTavernAppointmentCommandHandler> logger)
            {
                this.tavernAppointmentService = tavernAppointmentService;
                this.userService = userService;
                this.logger = logger;
            }

            public async Task<Result<Guid>> Handle(AddTavernAppointmentCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var userFromToken = JwtTokenHelper.DecodeToken(request.Token);

                    this.logger.LogInformation("Successfully decoded access token from request.");

                    if (userFromToken.Role == UserRoles.Admin)
                    {
                        var user = await this.userService.GetUserByEmailAsync(request.UserEmail);
                        var tavernAppointment = new KABINET_Domain.Entities.TavernAppointment(
                            user.Id,
                            TimeZoneInfo.ConvertTimeToUtc(request.StartTime),
                            TimeZoneInfo.ConvertTimeToUtc(request.EndTime),
                            request.Title);
                        user.AddTavernAppointment(tavernAppointment);
                        await this.tavernAppointmentService.AddAppointmentAsync(user);

                        this.logger.LogInformation("Successfully added tavern appointment for user with email {userEmail}, startTime {@startTime}, eventTitle {title}.", request.UserEmail, request.StartTime, request.Title);
                        return Result.Ok(tavernAppointment.Id);
                    }
                    else
                    {
                        this.logger.LogWarning("Error occurred in AddTavernAppointmentCommandHandler: User is not with Admin role!!");
                        return Result.Fail<Guid>("You do not have access to this information!");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error occurred in AddTavernAppointmentCommandHandler!");
                    return Result.Fail<Guid>("Internal Server Error!");
                }
            }
        }
    }
}
