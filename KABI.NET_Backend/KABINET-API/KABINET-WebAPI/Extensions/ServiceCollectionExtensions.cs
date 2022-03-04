using KABINET_Application.Boundaries.Http;
using KABINET_Application.Boundaries.Laundry;
using KABINET_Application.Boundaries.Logging;
using KABINET_Application.Boundaries.Notification;
using KABINET_Application.Boundaries.Reports;
using KABINET_Application.Boundaries.TavernAppointment;
using KABINET_Application.Boundaries.User;
using KABINET_Persistance;
using KABINET_Persistance.Services;
using KABINET_Persistance.Services.Notifications;
using KABINET_Persistance.Services.Reports;
using KABINET_Persistence.Services.Http;
using KABINET_Persistence.Services.Logging;
using KABINET_Persistence.Services.TavernAppointment;
using KABINET_Persistence.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KABINET_WebAPI.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services
                .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            return services;
        }

        internal static IServiceCollection InitializeDepdendencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Persistence:
            services.AddScoped<DbContext, KabinetDbContext>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ILaundryService, LaundryService>();
            services.AddTransient<IReportsService, ReportsService>();
            services.AddTransient<ITavernAppointmentService, TavernAppointmentService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IHttpContextService, HttpContextService>();
            services.AddTransient<ICorrelationIdService, CorrelationIdService>();

            return services;
        }

        internal static IServiceCollection InitializeDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<KabinetDbContext>(options =>
                    options.UseSqlServer(configuration.GetSection("DBConnectionString").Value));

            return services;
        }
    }
}
