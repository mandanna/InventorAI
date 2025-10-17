using System.ComponentModel.DataAnnotations;

namespace InventorAi_api.Models.DTO.Requests.Categories
{
    public class CategoryRequest
    {
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public int? StoreId { get; set; }
    }
}
