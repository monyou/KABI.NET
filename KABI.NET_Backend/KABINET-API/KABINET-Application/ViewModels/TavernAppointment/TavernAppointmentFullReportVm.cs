using System;

namespace KABINET_Application.ViewModels.TavernAppointment
{
    public class TavernAppointmentFullReportVm
    {
        public string EventDescription { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationInMinutes { get; set; }
        public string UserNames { get; set; }
        public string UserRoom { get; set; }
    }
}
