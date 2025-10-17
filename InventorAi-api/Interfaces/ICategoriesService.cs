using InventorAi_api.Models;
using InventorAi_api.Models.DTO.Requests.Categories;

namespace InventorAi_api.Interfaces
{
    public interface ICategoriesService
    {
        Task<ServiceResponse<CategoryResponse>> CreateCategory(CategoryRequest categoryCreateRequest);
        Task<ServiceResponse<CategoryResponse>> UpdateCategory(CategoryUpdateRequest categoryCreateRequest);
        Task<ServiceResponse<CategoryResponse>> GetCategoryById(int categoryId);
        Task<ServiceResponse<List<CategoryResponse>>> GetCategoryByStoreId(int categoryId);
        Task<ServiceResponse<List<CategoryResponse>>> GetActiveCategoriesBy(int storeId);
        Task<ServiceResponse<object>> ToggleCategoryStatus(int id);
        Task<ServiceResponse<object>> DeleteCategory(int id);
    }
}
