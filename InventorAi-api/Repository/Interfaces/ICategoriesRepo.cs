using InventorAi_api.EntityModels;
using InventorAi_api.Models;

namespace InventorAi_api.Repository.Interfaces
{
    public interface ICategoriesRepo
    {
        Task<Category> GetCategoryById(int id);
        Task<Category> GetCategory(string name, int storeId);
        Task<List<Category>> GetCategoryByStoreId(int storeId);
        Task<List<Category>> GetCommonCategory();
        Task<List<Category>> GetCommonAndStoreCategories(int storeId);
        void saveChangesAsync();
        Task<Category> Add(Category category);
        Task<List<Category>> GetActiveCategoryByStoreId(int storeId);
        void Delete(Category category);


    }
}
