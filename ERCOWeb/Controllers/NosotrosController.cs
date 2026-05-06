using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERCOWeb.Controllers
{
    public class NosotrosController : Controller
    {




        public IActionResult Index()
        {

            return View();
        }

    }
}
