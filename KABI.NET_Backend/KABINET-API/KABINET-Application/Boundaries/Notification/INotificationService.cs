using System.Threading.Tasks;

namespace KABINET_Application.Boundaries.Notification
{
    public interface INotificationService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
