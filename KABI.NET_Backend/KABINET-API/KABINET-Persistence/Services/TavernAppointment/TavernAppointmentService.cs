using KABINET_Application.Boundaries.TavernAppointment;
using KABINET_Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KABINET_Persistence.Services.TavernAppointment
{
    public class TavernAppointmentService : ITavernAppointmentService
    {
        private readonly KabinetDbContext kabinetDbContext;

        public TavernAppointmentService(
            KabinetDbContext kabinetDbContext)
        {
            this.kabinetDbContext = kabinetDbContext;
        }

        public async Task AddAppointmentAsync(KABINET_Domain.Entities.User user)
        {
            foreach (var appointment in user.TavernAppointments)
            {
                if (appointment.Id == Guid.Empty)
                {
                    kabinetDbContext.Entry(appointment).State = EntityState.Added;
                }
            }
            await kabinetDbContext.SaveChangesAsync();
        }

        public async Task<List<KABINET_Domain.Entities.TavernAppointment>> GetAllAppointments()
        {
            var appointments = new List<KABINET_Domain.Entities.TavernAppointment>();
            appointments = await this.kabinetDbContext.TavernAppointments
                .AsNoTracking()
                .Include(ta => ta.User)
                .Where(ta => ta.EndTime >= DateTime.UtcNow)
                .ToListAsync();

            return appointments;
        }

        public async Task<KABINET_Domain.Entities.TavernAppointment> GetAppointmentById(Guid tavernAppointmentId)
        {
            KABINET_Domain.Entities.TavernAppointment appointment;
            appointment = await this.kabinetDbContext.TavernAppointments
                .AsNoTracking()
                .Include(ta => ta.User)
                .FirstOrDefaultAsync(ta => ta.Id == tavernAppointmentId);

            return appointment;
        }
    }
}
