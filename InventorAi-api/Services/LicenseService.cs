using AutoMapper;
using Azure.Core;
using InventorAi_api.Data;
using InventorAi_api.EntityModels;
using InventorAi_api.Enums;
using InventorAi_api.Helpers;
using InventorAi_api.Interfaces;
using InventorAi_api.Models;
using InventorAi_api.Models.DTO.Requests.License;
using InventorAi_api.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using static InventorAi_api.Enums.ErrorCodes;

namespace InventorAi_api.Services
{
    public class LicenseService : ILicenseService
    {
        private readonly InventorAiContext _context;
        private readonly IMapper _mapper;
        private readonly ILicenseRepo _licenseRepo;
        public LicenseService(InventorAiContext context, IMapper mapper, ILicenseRepo licenseRepo)
        {
            _context = context;
            _mapper = mapper;
            _licenseRepo = licenseRepo;

        }

        public async Task<ServiceResponse<LicenseServiceResponse>> CreateLicenseForStore(LicenseRequest licenseRequest)
        {
            try
            {
                if (licenseRequest != null)
                {
                    var store = await _context.Stores
                    .Include(x => x.Licenses)
                    .FirstOrDefaultAsync(x => x.StoreId == licenseRequest.StoreId);
                    if (store == null)
                    {
                        return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = new ApiError { Message = "Store does not exits", Code = "STORE_NOT_FOUND" } };
                    }
                    var activeLicense = store.Licenses.FirstOrDefault(x => x.IsActive);
                    if (activeLicense != null)
                    {
                        return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = new ApiError { Message = "An active license already exists for this store", Code = "LICENSE_EXISTS" } };
                    }
                    var license = _mapper.Map<EntityModels.License>(licenseRequest);
                    _context.Licenses.Add(license);
                    await _context.SaveChangesAsync();
                    var licenseResponse = _mapper.Map<LicenseServiceResponse>(license);
                    return new ServiceResponse<LicenseServiceResponse> { Success = true, Data = licenseResponse };
                }
                else
                {
                    return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = new ApiError { Message = "Invalid license creation request" } };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }

        }

        public async Task<ServiceResponse<LicenseServiceResponse>> GetStoreLicenseAsync(int storeId)
        {
            try
            {
                if (storeId > 0)
                {
                    var response = await _licenseRepo.GetStoreLicenseAsync(storeId);
                    if (response == null)
                    {
                        return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = new ApiError { Code = "STORE_NOT_FOUND", Message = "Store does not exits" } };
                    }
                    var licenseResponse = _mapper.Map<LicenseServiceResponse>(response);
                    return new ServiceResponse<LicenseServiceResponse> { Data = licenseResponse, Success = true };
                }
                else
                {
                    return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = new ApiError { Message = "Invalid store id" } };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }
        }
        public async Task<ServiceResponse<LicenseServiceResponse>> GetLicenseByLicenseIdAsync(int licenseId)
        {
            try
            {
                if (licenseId > 0)
                {
                    var response = await _licenseRepo.GetLicenseByLicenseIdAsync(licenseId);
                    if (response == null)
                    {
                        return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = new ApiError { Code = "LICENSE_NOT_FOUND", Message = "Store does not exits" } };
                    }
                    var licenseResponse = _mapper.Map<LicenseServiceResponse>(response);
                    return new ServiceResponse<LicenseServiceResponse> { Data = licenseResponse, Success = true };
                }
                else
                {
                    return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = new ApiError { Message = "Invalid license id" } };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }
        }

        public async Task<ServiceResponse<LicenseServiceResponse>> UpdateStoreLicense(LicenseUpdateRequest licenseUpdateRequest)
        {
            try
            {
                if (licenseUpdateRequest != null)
                {

                    var license = await _licenseRepo.GetStoreLicenseAsync(licenseUpdateRequest.StoreId);

                    if (license == null)
                    {
                        return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = new ApiError { Code = "STORE_NOT_FOUND", Message = "Store does not exits or no valid license" } };
                    }
                    if (licenseUpdateRequest.IsRenew)
                    {
                        license.ExpiryDate = licenseUpdateRequest.NewExpiryDate;
                    }
                    if (licenseUpdateRequest.IsGenerateKey)
                    {
                        license.LicenseKey = Guid.NewGuid().ToString();
                    }
                    license.MaxUserCount = licenseUpdateRequest.MaxUserCount > 0 ? license.MaxUserCount : license.MaxUserCount;
                    license.IsActive = licenseUpdateRequest.IsActive ?? license.IsActive;

                    await _context.SaveChangesAsync();

                    var updatelicense = _mapper.Map<LicenseServiceResponse>(license);
                    return new ServiceResponse<LicenseServiceResponse> { Data = updatelicense, Success = true };
                }
                else
                {
                    return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = new ApiError { Code = "INVALID_LICENSE_UPDATION", Message = "Invalid license updation request" } };
                }
            }
            catch (Exception ex)
            {

                return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }
        }

        public async Task<ServiceResponse<object>> DeleteLicenseByStoreId(int storeId)
        {
            try
            {
                var license = await _licenseRepo.GetStoreLicenseAsync(storeId);
                if (license == null)
                {
                    return new ServiceResponse<object> { Success = false, Error = new ApiError { Code = "STORE_NOT_FOUND", Message = "Store does not exits or no valid license" } };
                }
                _context.Licenses.Remove(license);
                await _context.SaveChangesAsync();
                return new ServiceResponse<object> { Success = true };
            }
            catch (DbUpdateException dbex)
            {
                return new ServiceResponse<object> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, dbex.Message) };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }
        }
        public async Task<ServiceResponse<object>> ToggleLicenseStatus(int storeId)
        {
            try
            {
                var license = await _licenseRepo.GetStoreLicenseAsync(storeId);
                if (license == null)
                {
                    return new ServiceResponse<object> { Success = false, Error = new ApiError { Code = "STORE_NOT_FOUND", Message = "Store does not exits or no valid license" } };
                }
                license.IsActive = !license.IsActive;
                await _context.SaveChangesAsync();
                return new ServiceResponse<object> { Success = true, Message = $"License is {(license.IsActive == true ? "ACTIVE" : "Inactive")}" };
            }
            catch (DbUpdateException dbex)
            {
                return new ServiceResponse<object> { Success = false, Error = new ApiError { Message = dbex.Message } };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<object> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }

        }

        public async Task<ServiceResponse<List<LicenseServiceResponse>>> GetAllLicenses()
        {
            try
            {
                var licenses = await _licenseRepo.GetListOfLicenses();
                var mappedLicenses = _mapper.Map<List<LicenseServiceResponse>>(licenses);
                if (licenses == null)
                {
                    return new ServiceResponse<List<LicenseServiceResponse>> { Success = false, Error = new ApiError { Code = "LICENSE_NOT_FOUND", Message = "Licenses not found." } };
                }
                return new ServiceResponse<List<LicenseServiceResponse>> { Success = true, Data = mappedLicenses };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<LicenseServiceResponse>> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }
        }

        public async Task<ServiceResponse<LicenseServiceResponse>> ValidateLicenseByKey(string key)
        {
            try
            {
                var license = await _licenseRepo.GetLicenseByKeyAsync(key);
                if (license == null)
                {
                    return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Not_Found, "License not found.") };
                }
                if (!license.IsActive)
                {
                    return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InActive, "License is not active.") };
                }
                if (license.ExpiryDate < DateTime.UtcNow)
                {
                    return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Expired, "License is expired.") };
                }
                if (license.Store == null)
                {
                    return new ServiceResponse<LicenseServiceResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Not_Found, "Store not found for this license key.") };
                }
                var mappedResponse = _mapper.Map<LicenseServiceResponse>(license);
                return new ServiceResponse<LicenseServiceResponse> { Success = true, Data = mappedResponse };

            }
            catch (Exception ex)
            {

                return new ServiceResponse<LicenseServiceResponse> { Success = false, Error= ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }


        }
    }
}
