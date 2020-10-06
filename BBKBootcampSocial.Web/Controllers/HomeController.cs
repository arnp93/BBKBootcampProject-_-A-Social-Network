using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BBKBootcampSocial.Web.Controllers
{

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            
            return Ok();
        }
    }
}
