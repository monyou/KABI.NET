using System.Collections.Generic;
using System.Threading.Tasks;
using KABINET_Common;
using Microsoft.EntityFrameworkCore;
using KABINET_Persistance;
using System;
using KABINET_Application.Boundaries.User;
using KABINET_Application.Commands.User;

namespace KABINET_Persistence.Services.User
{
    public class UserService : IUserService
    {
        private readonly KabinetDbContext _ctx;

        public UserService(KabinetDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<KABINET_Domain.Entities.User>> GetAllUsersAsync()
        {
            List<KABINET_Domain.Entities.User> users;
            users = await _ctx.Users
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        public async Task<KABINET_Domain.Entities.User> GetUserByEmailAsync(string userEmail)
        {
            KABINET_Domain.Entities.User user;
            user = await _ctx.Users
                    .AsNoTracking()
                    .Include(u => u.Laundries)
                    .Include(u => u.TavernAppointments)
                    .FirstOrDefaultAsync(u => u.Email == userEmail);

            return user;
        }

        public async Task<KABINET_Domain.Entities.User> GetUserByIdAsync(string id)
        {
            KABINET_Domain.Entities.User user;
            user = await _ctx.Users
                .AsNoTracking()
                .Include(u => u.Laundries)
                .Include(u => u.TavernAppointments)
                .FirstOrDefaultAsync(u => u.Id.ToString() == id);

            return user;
        }

        public async Task<Guid> CreateUserAsync(RegisterUserCommand entry)
        {
            KABINET_Domain.Entities.User user;

            user = new KABINET_Domain.Entities.User(
               entry.Email,
               entry.FirstName,
               entry.LastName,
               entry.Room);

            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();

            return user.Id;
        }

        public async Task UpdateUserAsync(KABINET_Domain.Entities.User user)
        {
            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteUserByEmailAsync(string userEmail)
        {
            var user = await _ctx.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == userEmail);

            _ctx.Users.Remove(user);
            await _ctx.SaveChangesAsync();
        }

        public bool IsPasswordCorrect(string password, string hashedPassword)
        {
            return HashUtils.Verify(password, hashedPassword);
        }
    }
}
