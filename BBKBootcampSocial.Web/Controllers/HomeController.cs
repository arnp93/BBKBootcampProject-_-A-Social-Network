using BBKBootcampSocial.Domains.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.IServices;

namespace BBKBootcampSocial.Web.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IUserService UserService;

        public HomeController(IUserService UserService)
        {
            this.UserService = UserService;
        }
        public async Task<IActionResult> Index()
        {
            User user = new User
            {
                FirstName = "Arash",
                LastName = "NP",
                Email = "arash.n021@live.com",
                Password = "12345"
            };
            await UserService.AddUser(user);
            return Ok();
        }
    }
}
