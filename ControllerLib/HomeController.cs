using Microsoft.AspNetCore.Mvc;

namespace ControllerLib.Controllers
{
    [ControllerNamespaceConstraint]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content(RouteData.DataTokens.Count.ToString());
        }
    }
}