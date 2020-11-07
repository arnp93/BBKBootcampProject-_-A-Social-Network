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
using BBKBootcampSocial.Web.SignalR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.SignalR;
using BBKBootcampSocial.Core.DTOs.Notification;

namespace BBKBootcampSocial.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Constructor _ Dependency Injections

        private readonly IHubContext<NotificationHub> hubContext;

        private readonly IUserService UserService;

        public AccountController(IUserService UserService, IHubContext<NotificationHub> hubContext)
        {
            this.UserService = UserService;
            this.hubContext = hubContext;
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
                    CoverPic = user.CoverPic,
                    UserId = user.Id,
                    Notifications = await UserService.GetNotificationsOfUser(user.Id),
                    Friends = await UserService.GetFriendListByUserId(user.Id)
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
                        var test = new LoginUserInfoDTO
                        {
                            Token = tokenString,
                            ExpireTime = 30,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            ProfilePic = user.ProfilePic,
                            CoverPic = user.CoverPic,
                            UserId = user.Id,
                            Notifications = await UserService.GetNotificationsOfUser(user.Id),
                            Friends = await UserService.GetFriendListByUserId(user.Id)
                        };
                        return JsonResponseStatus.Success(test);
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

        #region Return User
        [HttpGet("view-profile/{userId}")]
        public async Task<IActionResult> ReturnUser(long userId)
        {
            return JsonResponseStatus.Success(await UserService.ReturnUserByIdWithPosts(userId));
        }

        #endregion

        #region Sign Out

        [HttpGet("sign-out")]
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #region Manage Friend Request

        [HttpPost("friend-request")]
        public async Task<IActionResult> FriendRequest([FromBody] long userId)
        {
            long currentUserId = User.GetUserId();
            var result = await UserService.AddFriend(userId, currentUserId);
            if (result.Item1)
            {
                string connectionId = await UserService.GetConnectionIdByUserId(userId);
                if (connectionId != "")
                {
                    await hubContext.Clients.Client(connectionId).SendAsync("AddFriendRequest", new NotificationDTO
                    {
                        Id = result.Item2.Id,
                        UserDestinationId = result.Item2.UserDestinationId,
                        UserOriginId = result.Item2.UserOriginId,
                        TypeOfNotification = result.Item2.TypeOfNotification,
                        IsAccepted = result.Item2.IsAccepted,
                        IsRead = result.Item2.IsRead
                    });
                }
                return JsonResponseStatus.Success();
            }
            else
            {
                return JsonResponseStatus.NotFound();
            }
               
        }

        [HttpPost("accept-friend")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] long originUserId)
        {
            long currentUserId = User.GetUserId();
            await UserService.AcceptFriend(currentUserId, originUserId);

            return JsonResponseStatus.Success();
        }

        #endregion

        #region Manage notification

        [HttpPost("delete-notification")]
        public async Task<IActionResult> DeleteNotification([FromBody]long notificationId)
        {
            await UserService.DeleteNotification(notificationId);

            return JsonResponseStatus.Success();
        }

        #endregion

        #region Get latest Users

        [HttpGet("latest-users")]
        public async Task<IActionResult> GetLastestUsers()
        {
            return JsonResponseStatus.Success(await UserService.GetLatestUsers());
        }

        #endregion
    }
}
