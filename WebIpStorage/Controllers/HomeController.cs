using System.Web.Mvc;

namespace WebIpStorage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult StoreIp()
        {
            ViewBag.Title = "Store IP";

            return View();
        }
    }
}