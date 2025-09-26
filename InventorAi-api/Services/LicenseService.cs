using InventorAi_api.Data;
using InventorAi_api.EntityModels;
using InventorAi_api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace InventorAi_api.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly InventorAiContext _context;
        public LicenseService(InventorAiContext context) {
            _context = context;
        }
        public async Task<EntityModels.License> GetUserLicenseAsync(int userId)
        {
            return await _context.Licenses.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<bool>IsLicenseValidAsync(int userId)
        {
            var license= await _context.Licenses.FirstOrDefaultAsync(x => x.UserId == userId);

            return license != null && license.IsActive && license.ExpiryDate >= DateTime.UtcNow;
        }
    }
}
