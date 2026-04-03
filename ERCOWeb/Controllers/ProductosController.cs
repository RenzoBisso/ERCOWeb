using Microsoft.AspNetCore.Mvc;

namespace ERCOWeb.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
