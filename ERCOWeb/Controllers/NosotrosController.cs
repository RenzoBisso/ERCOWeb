using ERCOWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERCOWeb.Controllers
{
    public class NosotrosController : Controller
    {

        


        public async Task<IActionResult> Index()
        {

            return View();
        }

    }
}
