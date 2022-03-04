using System;

namespace KABINET_Domain.Entities
{
    public class TavernAppointment
    {
        private DateTime startTime;
        private DateTime endTime;
        private string title;
        private Guid userId;

        public Guid Id { get; private set; }
        public DateTime StartTime
        {
            get => this.startTime;
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                this.startTime = value;
            }
        }
        public DateTime EndTime
        {
            get => this.endTime;
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                this.endTime = value;
            }
        }
        public string Title
        {
            get => this.title;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }

                this.title = value;
            }
        }
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
        public User User { get; set; }

        public TavernAppointment(
            Guid userId,
            DateTime startTime,
            DateTime endTime,
            string title)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Title = title;
            this.UserId = userId;
        }
    }
}
