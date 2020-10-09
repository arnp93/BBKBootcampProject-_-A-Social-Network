using BBKBootcampSocial.Domains.User;
using Microsoft.AspNetCore.Mvc;

namespace BBKBootcampSocial.Web.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult ActivateAccount(User user)
        {
            return View();
        }
    }
}
