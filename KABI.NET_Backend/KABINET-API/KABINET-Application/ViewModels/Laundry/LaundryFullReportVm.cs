using System;

namespace KABINET_Application.ViewModels.Laundry
{
    public class LaundryFullReportVm
    {
        public string User { get; set; }
        public string Room { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan LaundryTime { get; set; }
        public double ToPay { get; set; }
        public bool IsPaid { get; set; }
    }
}
