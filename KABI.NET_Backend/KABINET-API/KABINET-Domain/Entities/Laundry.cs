using System;

namespace KABINET_Domain.Entities
{
    public class Laundry
    {
        private Guid userId;

        public Guid Id { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }
        public double TotalToPay { get; private set; }
        public bool IsPaid { get; private set; }
        public TimeSpan TotalLaundryTime { get; private set; }
        public Guid UserId
        {
            get => this.userId;
            private set
            {
                if (value == Guid.Empty)
                {
                    throw new ArgumentNullException();
                }

                this.userId = value;
            }
        }
        public User User { get; private set; }

        public void StopLaundry(DateTime endTime, TimeSpan totalLaundryTime, double totalToPay)
        {
            this.EndTime = endTime;
            this.TotalLaundryTime = totalLaundryTime;
            this.TotalToPay = totalToPay;
        }

        public void PayLaundry()
        {
            this.IsPaid = true;
        }

        public Laundry(Guid userId)
        {
            this.StartTime = DateTime.UtcNow;
            this.EndTime = null;
            this.TotalToPay = 0;
            this.IsPaid = false;
            this.UserId = userId;
        }
    }
}
