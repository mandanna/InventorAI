using InventorAi_api.Data;
using InventorAi_api.EntityModels;
using InventorAi_api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventorAi_api.Repository
{
    public class LicenseRepo : ILicenseRepo
    {
        private readonly InventorAiContext _context;
        public LicenseRepo(InventorAiContext context)
        {
            _context = context;
        }

        public async Task<License> GetStoreLicenseAsync(int storeId)
        {
            return await _context.Licenses.FirstOrDefaultAsync(x => x.StoreId == storeId);
        }
        public async Task<License> GetLicenseByLicenseIdAsync(int licenseId)
        {
            return await _context.Licenses.FirstOrDefaultAsync(x => x.LicenseId == licenseId);
        }
        public async Task<List<License>> GetListOfLicenses()
        {
            return await _context.Licenses.Where(x=>!string.IsNullOrEmpty(x.LicenseKey)).ToListAsync();
        }
        public async Task<License> GetLicenseByKeyAsync(string key)
        {
            return await _context.Licenses.Include(x => x.Store).FirstOrDefaultAsync(x=>x.LicenseKey==key);
        
        }

    }
}
