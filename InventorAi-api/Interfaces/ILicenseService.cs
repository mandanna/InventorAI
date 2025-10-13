using InventorAi_api.EntityModels;
using InventorAi_api.Models;
using InventorAi_api.Models.DTO.Requests.License;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel;

namespace InventorAi_api.Interfaces
{
    public interface ILicenseService
    {
        Task<ServiceResponse<LicenseServiceResponse>> CreateLicenseForStore(LicenseRequest licenseRequest);
        Task<ServiceResponse<LicenseServiceResponse>> GetStoreLicenseAsync(int storeId);
        Task<ServiceResponse<LicenseServiceResponse>> UpdateStoreLicense(LicenseUpdateRequest licenseUpdateRequest);
        Task<ServiceResponse<object>> DeleteLicenseByStoreId(int storeId);
        Task<ServiceResponse<object>> ToggleLicenseStatus(int storeId);
        Task<ServiceResponse<List<LicenseServiceResponse>>> GetAllLicenses();
        Task<ServiceResponse<LicenseServiceResponse>> GetLicenseByLicenseIdAsync(int licenseId);
        Task<ServiceResponse<LicenseServiceResponse>> ValidateLicenseByKey(string key);
    }
}
