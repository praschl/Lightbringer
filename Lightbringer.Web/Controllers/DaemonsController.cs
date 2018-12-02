using Microsoft.AspNetCore.Mvc;

namespace Lightbringer.Web.Controllers
{
    public class DaemonsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}