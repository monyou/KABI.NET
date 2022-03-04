using System;

namespace KABINET_Application.ViewModels.Laundry
{
    public class LaundryVm
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double TotalToPay { get; set; }
        public bool IsPaid { get; set; }
        public TimeSpan TotalLaundryTime { get; set; }
        public KABINET_Domain.Entities.User User { get; set; }
    }
}
