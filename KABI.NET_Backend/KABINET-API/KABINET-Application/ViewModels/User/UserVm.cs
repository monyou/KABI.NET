using KABINET_Domain.Enums;
using System;

namespace KABINET_Application.ViewModels.User
{
    public class UserVm
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Room { get; set; }
        public UserRoles Role { get; set; }
        public int Warnings { get; set; }

        public UserVm() { }

        public UserVm(Guid id, string email, string firstName, string lastName, string room, int warnings, UserRoles role)
        {
            this.Id = id;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Room = room;
            this.Warnings = warnings;
            this.Role = role;
        }
    }
}
