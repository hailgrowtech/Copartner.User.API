using CopartnerUser.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopartnerUser.ServiceLayer.Services.Interfaces
{
    public interface IAvailabilityTypesService
    {
        Task<List<AvailabilityTypes>> GetAllAvailabilityTypesAsync();
        Task<AvailabilityTypes> GetAvailabilityTypeByIdAsync(int id);
        Task<int> AddAvailabilityTypeAsync(AvailabilityTypes availabilityTypes);
        Task<bool> UpdateAvailabilityTypeAsync(AvailabilityTypes availabilityTypes);
        Task<bool> DeleteAvailabilityTypeAsync(int id);
    }
}
