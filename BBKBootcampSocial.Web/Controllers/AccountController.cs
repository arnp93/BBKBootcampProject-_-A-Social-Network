using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.AllServices.IServices;
using BBKBootcampSocial.Core.DTOs.Account;
using BBKBootcampSocial.Core.Utilities;
using BBKBootcampSocial.Core.Utilities.Identity;
using Microsoft.IdentityModel.Tokens;
using BBKBootcampSocial.Domains.User;

namespace BBKBootcampSocial.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Constructor _ Dependency Injections

        private readonly IUserService UserService;

        public AccountController(IUserService UserService)
        {
            this.UserService = UserService;
        }

        #endregion

        #region Register

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO user)
        {
            var response = await UserService.AddUser(user);
            if (response == RegisterUserResult.EmailExists)
            {
                return JsonResponseStatus.Error(new { error = "Email Exist" });
            }

            return JsonResponseStatus.Success();
        }

        #endregion

        #region Check if User is Authenticated

        [HttpPost("check-auth")]
        public async Task<IActionResult> CheckAuth()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await UserService.GetUserById(User.GetUserId());

                return JsonResponseStatus.Success(new LoginUserInfoDTO
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePic = user.ProfilePic,
                    UserId = user.Id
                });
            
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO login)
        {
            if (ModelState.IsValid)
            {
                var response = await UserService.LoginUser(login);

                switch (response)
                {
                    case LoginUserResult.UserNotExist:
                        return JsonResponseStatus.NotFound();

                    case LoginUserResult.NotActivated:
                        return JsonResponseStatus.Error();

                    case LoginUserResult.Success:
                        var user = await UserService.GetUserByEmail(login.Email);
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BBKBootCampIssuerKeyJWTByArashNP"));
                        var signninCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                        var tokenOptions = new JwtSecurityToken(
                            issuer: "https://localhost:44317",
                            claims: new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Email),
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                            },
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: signninCredentials
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                        return JsonResponseStatus.Success(new LoginUserInfoDTO
                        {
                            Token = tokenString,
                            ExpireTime = 30,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            ProfilePic = user.ProfilePic,
                            UserId = user.Id
                        });
                }
            }
            return JsonResponseStatus.Error();
        }
        #endregion

        #region Active Account
        [HttpGet("activate-account/{activeCode}")]
        public async Task<IActionResult> ActiveUserFromEmail(string activeCode)
        {
            bool isSuccess = await UserService.ActiveAccount(activeCode);
            if (isSuccess)
                return Redirect("http://localhost:4200/");

            return Redirect("http://localhost:4200/active-error");


        }
        #endregion
    }
}
