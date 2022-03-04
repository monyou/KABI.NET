using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using KABINET_Application.Boundaries.Laundry;
using KABINET_Domain.Entities;

namespace KABINET_Persistance.Services
{
    public class LaundryService : ILaundryService
    {
        private readonly KabinetDbContext kabinetDbContext;

        public LaundryService(
            KabinetDbContext kabinetDbContext)
        {
            this.kabinetDbContext = kabinetDbContext;
        }

        public async Task<Laundry> GetLaundryByIdAsync(Guid laundryId)
        {
            Laundry laundry = null;
            laundry = await kabinetDbContext.Laundries
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l => l.Id == laundryId);

            return laundry;
        }

        public async Task<Laundry> GetLastRecordAsync()
        {
            Laundry laundry = null;
            laundry = await kabinetDbContext.Laundries
                .AsNoTracking()
                .Include(l => l.User)
                .OrderBy(l => l.StartTime)
                .LastOrDefaultAsync();

            return laundry;
        }

        public async Task StartLaundryAsync(User user)
        {
            foreach (var laundry in user.Laundries)
            {
                if (laundry.Id == Guid.Empty)
                {
                    kabinetDbContext.Entry(laundry).State = EntityState.Added;
                }
            }
            await kabinetDbContext.SaveChangesAsync();
        }

        public async Task UpdateLaundryAsync(Laundry laundry)
        {
            kabinetDbContext.Laundries.Update(laundry);
            await kabinetDbContext.SaveChangesAsync();
        }
    }
}
