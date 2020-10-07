using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.IServices;
using BBKBootcampSocial.Core.DTOs.Account;
using BBKBootcampSocial.Core.Utilities;

namespace BBKBootcampSocial.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService UserService;

        public AccountController(IUserService UserService)
        {
            this.UserService = UserService;
        }
      
        public async Task<IActionResult> RegisterUser([FromBody]RegisterUserDTO user)
        {
            var response = await UserService.AddUser(user);
            if (response == RegisterUserResult.EmailExists)
            {
                return JsonResponseStatus.Error(new {error = "Email Exist"});
            }

            return JsonResponseStatus.Success();
        }
    }
}
