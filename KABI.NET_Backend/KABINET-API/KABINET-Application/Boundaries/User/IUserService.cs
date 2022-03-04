using KABINET_Application.Commands.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KABINET_Application.Boundaries.User
{
    public interface IUserService
    {
        Task<List<KABINET_Domain.Entities.User>> GetAllUsersAsync();
        Task<KABINET_Domain.Entities.User> GetUserByEmailAsync(string userEmail);
        Task<KABINET_Domain.Entities.User> GetUserByIdAsync(string userId);
        Task<Guid> CreateUserAsync(RegisterUserCommand entry);
        Task UpdateUserAsync(KABINET_Domain.Entities.User user);
        Task DeleteUserByEmailAsync(string userEmail);
        bool IsPasswordCorrect(string password, string hashedPassword);
    }
}
