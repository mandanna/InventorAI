using System.ComponentModel.DataAnnotations;

namespace InventorAi_api.Models
{
    public class CategoryUpdateRequest
    {
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public int? StoreId { get; set; }
        public int CategoryId { get; set; }
    }
}
