using InventorAi_api.Enums;
using InventorAi_api.Helpers;
using InventorAi_api.Interfaces;
using InventorAi_api.Models;
using InventorAi_api.Models.DTO.Requests.Categories;
using InventorAi_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static InventorAi_api.Enums.ErrorCodes;

namespace InventorAi_api.Controllers.Category
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;

        }
        //[Authorize(Roles = "Admin")]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CategoryRequest categoryCreateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCodes.ErrorCode.Invalid_Input, "One or more required fields are missing")
                });
            }
            var reponse = await _categoriesService.CreateCategory(categoryCreateRequest);
            return Ok(reponse);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateRequest categoryCreateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCodes.ErrorCode.Invalid_Input, "One or more required fields are missing")
                });
            }
            var reponse = await _categoriesService.UpdateCategory(categoryCreateRequest);
            return Ok(reponse);
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            if (categoryId<=0)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCodes.ErrorCode.Invalid_Input, "One or more required fields are missing")
                });
            }
            var reponse = await _categoriesService.GetCategoryById(categoryId);
            return Ok(reponse);
        }

        [HttpGet("GetCategoryByStoreId")]
        public async Task<IActionResult> GetCategoryByStoreId(int storeId)
        {
            if (storeId <= 0)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCodes.ErrorCode.Invalid_Input, "One or more required fields are missing")
                });
            }
            var reponse = await _categoriesService.GetCategoryByStoreId(storeId);
            return Ok(reponse);
        }
        [HttpGet("GetActiveCategoriesBy")]
        public async Task<IActionResult> GetActiveCategoriesBy(int storeId ) {
            if (storeId <= 0)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCodes.ErrorCode.Invalid_Input, "One or more required fields are missing")
                });
            }
            var reponse = await _categoriesService.GetActiveCategoriesBy(storeId);
            return Ok(reponse);
        }
        [HttpPut("ToggleLicenseStatus")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleCategoryStatus(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Category id is missing")
                });
            }
            var reponse = await _categoriesService.ToggleCategoryStatus(id);
            return Ok(reponse);
        }
        [HttpDelete("DeleteCategory")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ServiceResponse<object>
                {
                    Success = false,
                    Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Category id is missing")
                });
            }
            var reponse = await _categoriesService.DeleteCategory(id);
            return Ok(reponse);
        }
    }
}
