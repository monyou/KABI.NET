using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KABINET_Application.Boundaries.TavernAppointment
{
    public interface ITavernAppointmentService
    {
        Task AddAppointmentAsync(KABINET_Domain.Entities.User user);
        Task<KABINET_Domain.Entities.TavernAppointment> GetAppointmentById(Guid tavernAppointmentId);
        Task<List<KABINET_Domain.Entities.TavernAppointment>> GetAllAppointments();
    }
}
