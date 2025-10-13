using InventorAi_api.EntityModels;

namespace InventorAi_api.Repository.Interfaces
{
    public interface ILicenseRepo
    {
        Task<License> GetStoreLicenseAsync(int storeId);
        Task<List<License>> GetListOfLicenses();
        Task<License> GetLicenseByLicenseIdAsync(int licenseId);
        Task<License> GetLicenseByKeyAsync(string key);
    }
}
