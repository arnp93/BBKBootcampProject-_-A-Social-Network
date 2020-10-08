using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.IServices;
using BBKBootcampSocial.Core.DTOs.Account;
using BBKBootcampSocial.Core.Utilities;
using Microsoft.IdentityModel.Tokens;

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

        #region Login
        [HttpPost]
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
                        return JsonResponseStatus.Error(new { message = "Tu cuenta no esta activo" });

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
                            Token = tokenString, ExpireTime = 30, FirstName = user.FirstName, LastName = user.LastName,
                            UserId = user.Id
                        });
                }
            }
            return JsonResponseStatus.Error();
        }
        #endregion
    }
}
