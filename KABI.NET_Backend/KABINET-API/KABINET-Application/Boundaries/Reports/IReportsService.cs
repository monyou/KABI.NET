using System.Threading.Tasks;

namespace KABINET_Application.Boundaries.Reports
{
    public interface IReportsService
    {
        Task SendLaundryFullReportAsync();
        Task SendTavernAppointmentFullReportAsync();
    }
}
