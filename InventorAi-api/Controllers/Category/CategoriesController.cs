using Microsoft.AspNetCore.Mvc;

namespace InventorAi_api.Controllers.Category
{
    public class CategoriesController : Controller
    {
        public CategoriesController() {
        
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
    }
}
