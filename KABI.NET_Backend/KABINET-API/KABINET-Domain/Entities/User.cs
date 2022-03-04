using KABINET_Common;
using KABINET_Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace KABINET_Domain.Entities
{
    public class User
    {
        private string email;
        private string firstName;
        private string lastName;
        private string room;
        private string password;
        private List<Laundry> laundries = new List<Laundry>();
        private List<TavernAppointment> tavernAppointments = new List<TavernAppointment>();

        public Guid Id { get; private set; }
        public string Email
        {
            get => this.email;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }

                this.email = value;
            }
        }
        public string FirstName
        {
            get => this.firstName;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }

                this.firstName = value;
            }
        }
        public string LastName
        {
            get => this.lastName;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }

                this.lastName = value;
            }
        }
        public string Room
        {
            get => this.room;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }

                this.room = value;
            }
        }
        public string Password
        {
            get => this.password;
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }

                this.password = value;
            }
        }
        public UserRoles Role { get; private set; }
        public int Warnings { get; private set; }
        [JsonIgnore]
        public IReadOnlyCollection<Laundry> Laundries => this.laundries;
        [JsonIgnore]
        public IReadOnlyCollection<TavernAppointment> TavernAppointments => this.tavernAppointments;

        public void AddLaundry(Laundry laundry)
        {
            this.laundries.Add(laundry);
        }

        public void AddTavernAppointment(TavernAppointment tavernAppointment)
        {
            if (Role == UserRoles.Blocked)
            {
                throw new ArgumentException();
            }

            if (tavernAppointment.StartTime < DateTime.UtcNow)
            {
                throw new ArgumentException();
            }

            var appointmentExists = false;

            foreach (var ta in this.TavernAppointments)
            {
                if ((ta.StartTime >= tavernAppointment.StartTime && ta.StartTime <= tavernAppointment.EndTime) ||
                    (ta.EndTime >= tavernAppointment.StartTime && ta.EndTime <= tavernAppointment.EndTime))
                {
                    appointmentExists = true;
                    break;
                }
            }

            if (appointmentExists)
            {
                throw new ArgumentException();
            }

            this.tavernAppointments.Add(tavernAppointment);
        }

        public void ChangePassword(string newPassword)
        {
            if (Role == UserRoles.Blocked)
            {
                throw new ArgumentException();
            }

            this.password = newPassword;
        }

        public void AddWarning()
        {
            if (this.Warnings < 3)
            {
                this.Warnings++;
            }

            if (this.Warnings == 3)
            {
                this.Role = UserRoles.Blocked;
            }
        }

        public User(
            string email,
            string firstName,
            string lastName,
            string room,
            UserRoles role = UserRoles.Member)
        {
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Room = room;
            this.Role = role;
            this.Password = HashUtils.Hash(room);
        }

        public User(
           Guid id,
           string email,
           string firstName,
           string lastName,
           string room,
           UserRoles role = UserRoles.Member)
        {
            this.Id = id;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Room = room;
            this.Role = role;
            this.Password = HashUtils.Hash(room);
        }
    }
}
