using Microsoft.AspNetCore.Mvc;

namespace People2.Controllers
{
    public class Contact : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
