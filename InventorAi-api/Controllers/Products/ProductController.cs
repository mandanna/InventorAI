using Microsoft.AspNetCore.Mvc;

namespace InventorAi_api.Controllers.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
