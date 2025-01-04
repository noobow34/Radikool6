using Microsoft.AspNetCore.Mvc;

namespace Radikool6.Controllers
{
    public class DummyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
