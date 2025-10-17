using InventorAi_api.Data;
using InventorAi_api.EntityModels;
using InventorAi_api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventorAi_api.Repository
{
    public class CategoriesRepo : ICategoriesRepo
    {
        private readonly InventorAiContext _context;
        public CategoriesRepo(InventorAiContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category;
        }

        public async Task<Category> GetCategory(string name, int storeId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => (storeId > 0 ? x.StoreId == storeId && x.CategoryName == name : (x.StoreId == null || x.StoreId == 0) && x.CategoryName == name));
            return category;
        }

        public async Task<List<Category>> GetCategoryByStoreId(int storeId)
        {
            var categories = await _context.Categories.Where(x => storeId > 0 ? x.StoreId == storeId : x.StoreId == null).ToListAsync();
            return categories;
        }

        public async Task<List<Category>> GetCommonCategory()
        {
            var categories = await _context.Categories.Where(x => x.StoreId == null).ToListAsync();
            return categories;
        }
        public async Task<List<Category>> GetCommonAndStoreCategories(int storeId)
        {
            var categories = await _context.Categories.Where(x => x.StoreId == storeId || x.StoreId == null).ToListAsync();
            return categories;
        }
        public async Task<List<Category>> GetActiveCategoryByStoreId(int storeId)
        {
            var categories = await _context.Categories
           .Where(x => x.Store == null || x.Store.Licenses.Any(l => l.IsActive)).ToListAsync();
            return categories;
        }

        public void saveChangesAsync()
        {
            _context.SaveChangesAsync();
        }

        public async Task<Category> Add(Category category)
        {
            await _context.AddAsync(category);
            saveChangesAsync();
            return category;
        }
        public async void Delete(Category category)
        {
            _context.Categories.Remove(category);
            saveChangesAsync();
        }
    }
}
