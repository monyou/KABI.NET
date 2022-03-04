using System;
using System.Threading.Tasks;

namespace KABINET_Application.Boundaries.Laundry
{
    public interface ILaundryService
    {
        Task StartLaundryAsync(KABINET_Domain.Entities.User user);
        Task UpdateLaundryAsync(KABINET_Domain.Entities.Laundry laundry);
        Task<KABINET_Domain.Entities.Laundry> GetLaundryByIdAsync(Guid laundryId);
        Task<KABINET_Domain.Entities.Laundry> GetLastRecordAsync();
    }
}
