using AutoMapper;
using InventorAi_api.Data;
using InventorAi_api.EntityModels;
using InventorAi_api.Enums;
using InventorAi_api.Helpers;
using InventorAi_api.Interfaces;
using InventorAi_api.Models;
using InventorAi_api.Models.DTO.Requests.Categories;
using InventorAi_api.Repository;
using InventorAi_api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using static InventorAi_api.Enums.ErrorCodes;

namespace InventorAi_api.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly InventorAiContext _context;
        private readonly ICategoriesRepo _repo;
        private readonly IMapper _mapper;
        public CategoriesService(InventorAiContext context, IMapper mapper, ICategoriesRepo categoriesRepo)
        {
            _context = context;
            _mapper = mapper;
            _repo = categoriesRepo;

        }

        public async Task<ServiceResponse<CategoryResponse>> CreateCategory(CategoryRequest categoryCreateRequest)
        {
            try
            {
                if (categoryCreateRequest == null)
                {
                    return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Invalid category creation request.") };
                }
                var category = await _repo.GetCategory(categoryCreateRequest.CategoryName, categoryCreateRequest.StoreId ?? 0);
                if (category != null)
                {
                    return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Duplicate category creation request.") };
                }
                var mappedCategory = _mapper.Map<Category>(categoryCreateRequest);
                await _repo.Add(mappedCategory);
                var response = _mapper.Map<CategoryResponse>(mappedCategory);
                return new ServiceResponse<CategoryResponse> { Data = response, Success = true };
            }
            catch (DbException dbex)
            {
                return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.DbException, dbex.Message) };


            }
            catch (Exception ex)
            {

                return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }

        }

        public async Task<ServiceResponse<CategoryResponse>> UpdateCategory(CategoryUpdateRequest categoryUpdateRequest)
        {
            try
            {
                if (categoryUpdateRequest == null)
                {
                    return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Invalid category updation request.") };
                }
                var category = await _repo.GetCategoryById(categoryUpdateRequest.CategoryId);
                if (category == null)
                {
                    return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Not_Found, "Category not found.") };
                }
                var mappedCategory = _mapper.Map(categoryUpdateRequest, category);
                _repo.saveChangesAsync();
                var response = _mapper.Map<CategoryResponse>(category);
                return new ServiceResponse<CategoryResponse> { Data = response, Success = true };
            }
            catch (DbException dbex)
            {
                return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.DbException, dbex.Message) };


            }
            catch (Exception ex)
            {

                return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }
        }
        public async Task<ServiceResponse<CategoryResponse>> GetCategoryById(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                {
                    return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Category id is required.") };
                }
                var category = await _repo.GetCategoryById(categoryId);
                if (category == null)
                {
                    return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Not_Found, "Category not found.") };
                }
                var response = _mapper.Map<CategoryResponse>(category);
                return new ServiceResponse<CategoryResponse> { Data = response, Success = true };

            }
            catch (Exception ex)
            {

                return new ServiceResponse<CategoryResponse> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }
        }
        public async Task<ServiceResponse<List<CategoryResponse>>> GetCategoryByStoreId(int storeId)
        {
            try
            {
                if (storeId <= 0)
                {
                    return new ServiceResponse<List<CategoryResponse>> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Store id is required.") };
                }
                var categories = await _repo.GetCommonAndStoreCategories(storeId);
                if (categories.Count <= 0)
                {
                    return new ServiceResponse<List<CategoryResponse>> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Not_Found, "No categories found for this storeid.") };
                }
                var response = _mapper.Map<List<CategoryResponse>>(categories);
                return new ServiceResponse<List<CategoryResponse>> { Data = response, Success = true };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<CategoryResponse>> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }
        }
        public async Task<ServiceResponse<List<CategoryResponse>>> GetActiveCategoriesBy(int storeId)
        {
            try
            {
                if (storeId <= 0)
                {
                    return new ServiceResponse<List<CategoryResponse>> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Store id is required.") };
                }
                var categories = await _repo.GetActiveCategoryByStoreId(storeId);
                if (categories.Count <= 0)
                {
                    return new ServiceResponse<List<CategoryResponse>> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Not_Found, "No categories found for this storeid.") };
                }
                var response = _mapper.Map<List<CategoryResponse>>(categories);
                return new ServiceResponse<List<CategoryResponse>> { Data = response, Success = true };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<CategoryResponse>> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.InternalServerError, ex.Message) };
            }

        }
        public async Task<ServiceResponse<object>> ToggleCategoryStatus(int id)
        {
            try
            {
                var category = await _repo.GetCategoryById(id);
                if (category == null)
                {
                    return new ServiceResponse<object> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Category does not exits.") };
                }
                category.IsActive = !category.IsActive;
                _repo.saveChangesAsync();
                return new ServiceResponse<object> { Success = true, Message = $"Category is {(category.IsActive == true ? "Active" : "Inactive")}" };
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
        public async Task<ServiceResponse<object>> DeleteCategory(int id)
        {
            try
            {
                var category = await _repo.GetCategoryById(id);
                if (category == null)
                {
                    return new ServiceResponse<object> { Success = false, Error = ErrorHelper.FromErrorCode(ErrorCode.Invalid_Input, "Category does not exits.") };
                }
                _repo.Delete(category);
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

    }
}
