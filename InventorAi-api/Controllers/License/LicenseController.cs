using InventorAi_api.EntityModels;
using InventorAi_api.Helpers;
using InventorAi_api.Interfaces;
using InventorAi_api.Models;
using InventorAi_api.Models.DTO.Requests.License;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static InventorAi_api.Enums.ErrorCodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventorAi_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseService _licenseService;
        public LicenseController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpPost("CreateLicenseForStore")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLicenseForStore([FromBody] LicenseRequest licenseRequest)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "One or more required fields are missing")
                });
            }
            var response = await _licenseService.CreateLicenseForStore(licenseRequest);
            return Ok(response);
        }
        [HttpGet("GetLicenseByStoreId")]
        //[Authorize(Roles ="Admin,Manager")]
        public async Task<IActionResult> GetLicenseByStoreId(int storeId)
        {
            if (storeId <= 0)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Store id is missing")
                });
            }
            var response = await _licenseService.GetStoreLicenseAsync(storeId);
            return Ok(response);
        }
        [HttpPut("UpdateStoreLicense")]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateStoreLicense([FromBody] LicenseUpdateRequest licenseUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "One or more required fields are missing")
                });
            }
            var response = await _licenseService.UpdateStoreLicense(licenseUpdateRequest);
            return Ok(response);
        }

        [HttpDelete("DeleteLicense")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLicense(int storeId)
        {
            if (storeId <= 0)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Store id is missing")
                });
            }
            var reponse = await _licenseService.DeleteLicenseByStoreId(storeId);
            return Ok(reponse);

        }

        [HttpPut("ToggleLicenseStatus")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleLicenseStatus(int storeId)
        {
            if (storeId <= 0)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Store id is missing")
                });
            }
            var reponse = await _licenseService.ToggleLicenseStatus(storeId);
            return Ok(reponse);
        }

        [HttpGet("GetAllLicenses")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllLicenses()
        {
            var reponse = await _licenseService.GetAllLicenses();
            return Ok(reponse);
        }
        [HttpGet("GetLicenseByLicenseId")]
        //[Authorize(Roles ="Admin,Manager")]
        public async Task<IActionResult> GetLicenseByLicenseId(int licenseId)
        {
            if (licenseId <= 0)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "License id is missing")
                });
            }
            var response = await _licenseService.GetLicenseByLicenseIdAsync(licenseId);
            return Ok(response);
        }
        [HttpGet("ValidateLicenseFromKey")]
        public async Task<IActionResult> ValidateLicenseFromKey(string key)
        {
            if (string.IsNullOrEmpty(key)){

                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input,"License key is missing")
                });            
            }
            var response = await _licenseService.ValidateLicenseByKey(key);
            return Ok(response);
        }
    }
}