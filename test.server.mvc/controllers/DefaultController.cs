using System.Security.Claims;
using System.Web.Mvc;

namespace test.server.mvc.controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
		  [Authorize]
        public ActionResult Index()
        {
            return View((User as ClaimsPrincipal).Claims);
        }
    }
}