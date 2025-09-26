using InventorAi_api.EntityModels;
using System.IdentityModel;

namespace InventorAi_api.Interfaces
{
    public interface ILicenseService
    {
        Task<bool> IsLicenseValidAsync(int userId);
        Task<License> GetUserLicenseAsync(int userId);
    }
}
