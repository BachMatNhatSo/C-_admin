using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace WebsiteAdmin.Controllers
{
    public class TreeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Welcome(string name , int numTimes=1)
        {
            ViewData["Message"] = "Hello" +name;
            ViewData["numTime"] = numTimes;
            return View();
        }
    }
}
