using System;
using BBKBootcampSocial.DataLayer.Implementations;
using System.Threading.Tasks;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.Domains.User;
using BBKBootcampSocial.Core.DTOs.Account;
using AutoMapper;
using System.Linq;
using BBKBootcampSocial.Core.AllServices.IServices;
using BBKBootcampSocial.Core.Security;
using BBKBootcampSocial.Core.Utilities.Convertors;
using BBKBootcampSocial.Domains.Access;
using Microsoft.EntityFrameworkCore;
using BBKBootcampSocial.Core.DTOs.Post;
using BBKBootcampSocial.Core.DTOs.Comment;
using BBKBootcampSocial.Domains.Common_Entities;
using System.Collections.Generic;
using BBKBootcampSocial.Core.DTOs.Notification;

namespace BBKBootcampSocial.Core.AllServices.Services
{
    public class UserService : IUserService
    {
        #region Constructor

        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        private IMailSender mailSender;
        private IViewRenderService viewRenderService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IMailSender mailSender, IViewRenderService viewRenderService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.mailSender = mailSender;
            this.viewRenderService = viewRenderService;
        }

        #endregion

        #region Register
        public async Task<RegisterUserResult> AddUser(RegisterUserDTO user)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            #region Sanitize Properties of user (RegisterUserDTO) || Secutiry

            user.Email = user.Email.ToLower().Trim().SanitizeText();
            user.FirstName = user.FirstName.SanitizeText();
            user.LastName = user.LastName.SanitizeText();
            user.Password = user.Password.SanitizeText();
            user.RePassword = user.RePassword.SanitizeText();

            #endregion

            #region Save User in Database

            User User = mapper.Map<User>(user);
            User.Password = PasswordHelper.EncodePasswordMd5(User.Password);
            if (await IsEmailExist(User.Email))
            {
                return RegisterUserResult.EmailExists;
            }

            User.ActiveCode = Guid.NewGuid().ToString() + "BBK-BootCamp";
            User.Username = $"BBK{User.Email}Bootcamp";
            await repository.AddEntity(User);
            await unitOfWork.SaveChanges();

            #endregion

            #region Add Users Role

            var repostoryForRoles = await unitOfWork.GetRepository<GenericRepository<UserRole>, UserRole>();
            await repostoryForRoles.AddEntity(new UserRole
            {
                IsDelete = false,
                RoleId = 1,
                UserId = User.Id
            });
            await unitOfWork.SaveChanges();

            #endregion

            #region Send Active Email

            string body = await viewRenderService.RenderToStringAsync("Email/ActivateAccount", User);
            mailSender.Send(user.Email, "BBK Bootcamp Social Network - Activate your Account", body);
            return RegisterUserResult.Success;

            #endregion

        }

        #endregion

        #region Login

        public async Task<LoginUserResult> LoginUser(LoginUserDTO login)
        {
            login.Email = login.Email.ToLower().Trim();
            try
            {
                if (!await IsUserExist(login.Email, login.Password))
                    return LoginUserResult.UserNotExist;

                if (!await IsUserActive(login.Email))
                    return LoginUserResult.NotActivated;

                if (await ValidateEmailAndPassword(login.Email, login.Password))
                    return LoginUserResult.Success;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return LoginUserResult.UnknownError;
        }

        public async Task<List<FriendDTO>> GetFriendListByUserId(long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<UserFriend>, UserFriend>();
            var userRepository = await unitOfWork.GetRepository<GenericRepository<User>, User>();
            return repository.GetEntitiesQuery().Where(fl => fl.UserId == userId).Select(f => new FriendDTO
            {
                UserId = f.FriendUserId,
                UserName = userRepository.GetEntityById(f.FriendUserId).Result.FirstName + " " + userRepository.GetEntityById(f.FriendUserId).Result.LastName,
                ProfilePicture = userRepository.GetEntityById(f.FriendUserId).Result.ProfilePic
            }).ToList();
        }

        #endregion

        #region Active Account

        public async Task<bool> ActiveAccount(string activeCode)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            User user = repository.GetEntitiesQuery().FirstOrDefault(u => u.ActiveCode == activeCode);

            if (user != null)
            {
                user.ActiveCode = Guid.NewGuid().ToString() + "BBK-Bootcamp";
                user.IsActive = true;
                repository.UpdateEntity(user);
                await unitOfWork.SaveChanges();
                return true;
            }

            return false;
        }

        #endregion

        #region Manage Friend Request

        public async Task<Tuple<bool, NotificationDTO>> AddFriend(long userId, long currentUserId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            var notificationRepository = await unitOfWork.GetRepository<GenericRepository<Notification>, Notification>();
            var notificationDTO = new NotificationDTO();
            if (repository.GetEntitiesQuery().Any(u => u.Id == userId))
            {
                if (notificationRepository.GetEntitiesQuery()
                    .Any(n => n.UserDestinationId == userId && n.UserOriginId == currentUserId && n.IsDelete))
                {
                    Notification notification = notificationRepository
                        .GetEntitiesQuery().FirstOrDefault(n => n.UserDestinationId == userId && n.UserOriginId == currentUserId && n.IsDelete);
                    notification.IsDelete = false;
                    notificationRepository.UpdateEntity(notification);
                    await unitOfWork.SaveChanges();
                    notificationDTO = new NotificationDTO
                    {
                        Id = notification.Id,
                        UserDestinationId = notification.UserDestinationId,
                        FirstName = GetUserById(notification.UserOriginId).GetAwaiter().GetResult().FirstName,
                        LastName = GetUserById(notification.UserOriginId).GetAwaiter().GetResult().LastName,
                        ImageName = GetUserById(notification.UserOriginId).GetAwaiter().GetResult().ProfilePic,
                        Message = "Has sent you a friend request",
                        UserOriginId = notification.UserOriginId,
                        TypeOfNotification = notification.TypeOfNotification,
                        IsAccepted = notification.IsAccepted,
                        IsRead = notification.IsRead
                    };
                }
                else
                {

                    Notification notification = new Notification
                    {
                        UserOriginId = currentUserId,
                        UserDestinationId = userId,
                        IsRead = false,
                        IsDelete = false,
                        IsAccepted = false,
                        TypeOfNotification = TypeOfNotification.FriendRequest
                    };
                    await notificationRepository.AddEntity(notification);
                    await unitOfWork.SaveChanges();
                    notificationDTO = new NotificationDTO
                    {
                        Id = notification.Id,
                        UserDestinationId = notification.UserDestinationId,
                        FirstName = GetUserById(notification.UserOriginId).GetAwaiter().GetResult().FirstName,
                        LastName = GetUserById(notification.UserOriginId).GetAwaiter().GetResult().LastName,
                        ImageName = GetUserById(notification.UserOriginId).GetAwaiter().GetResult().ProfilePic,
                        Message = "Has sent you a friend request",
                        UserOriginId = notification.UserOriginId,
                        TypeOfNotification = notification.TypeOfNotification,
                        IsAccepted = notification.IsAccepted,
                        IsRead = notification.IsRead
                    };
                }

                return new Tuple<bool, NotificationDTO>(true, notificationDTO);
            }
            else
            {
                return new Tuple<bool, NotificationDTO>(true, null);
            }
        }

        public async Task AcceptFriend(long currentUserId, long originUserId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();
            var friendRepository = await unitOfWork.GetRepository<GenericRepository<UserFriend>, UserFriend>();
            var notificationRepository = await unitOfWork.GetRepository<GenericRepository<Notification>, Notification>();

            var notificationOfRequest = notificationRepository.GetEntitiesQuery().SingleOrDefault(n =>
                n.UserDestinationId == currentUserId && n.UserOriginId == originUserId &&
                n.TypeOfNotification == TypeOfNotification.FriendRequest && !n.IsDelete);

            if (notificationOfRequest != null)
                notificationOfRequest.IsAccepted = true;

            User currentUser = await repository.GetEntityById(currentUserId);
            await friendRepository.AddEntity(new UserFriend
            {
                FriendUserId = currentUserId,
                UserId = originUserId,
                IsDelete = false
            });

            repository.UpdateEntity(currentUser);
            notificationRepository.UpdateEntity(notificationOfRequest);

            await unitOfWork.SaveChanges();
        }

        public async Task DeleteFriendRequest(long userDestinationId, long userId)
        {
            var notificationRepository = await unitOfWork.GetRepository<GenericRepository<Notification>, Notification>();

            if (notificationRepository.GetEntitiesQuery()
                .Any(n => n.UserDestinationId == userDestinationId && n.UserOriginId == userId && n.TypeOfNotification == TypeOfNotification.FriendRequest && !n.IsDelete))
            {
                Notification notification = notificationRepository
                    .GetEntitiesQuery().FirstOrDefault(n => n.UserDestinationId == userDestinationId && n.UserOriginId == userId &&
                                                            n.TypeOfNotification == TypeOfNotification.FriendRequest && !n.IsDelete);

                notificationRepository.RemoveEntity(notification);

                await unitOfWork.SaveChanges();
            }

        }

        #endregion

        #region Tools

        public async Task<bool> IsEmailExist(string email)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return repository.GetEntitiesQuery().Any(u => u.Email == email);

        }

        public async Task<bool> ValidateEmailAndPassword(string email, string password)
        {
            User user = await GetUserByEmail(email);

            if (user != null)
            {
                if (user.Password == PasswordHelper.EncodePasswordMd5(password))
                    return true;

                return false;
            }
            return false;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return repository.GetEntitiesQuery().FirstOrDefault(u => u.Email == email);
        }

        public async Task<bool> IsUserActive(string email)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return repository.GetEntitiesQuery().Any(u => u.Email == email && u.IsActive);
        }

        public async Task<User> GetUserById(long id)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return repository.GetEntitiesQuery().FirstOrDefault(u => u.Id == id);
        }

        public async Task<bool> IsUserExist(string email, string password)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return repository.GetEntitiesQuery().Any(u => u.Email == email && u.Password == PasswordHelper.EncodePasswordMd5(password));
        }

        public async Task<LoginUserInfoDTO> ReturnUserByIdWithPosts(long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            User user = repository.GetEntitiesQuery().Where(u => u.Id == userId).Include(u => u.Posts)
                .ThenInclude(p => p.Comments).ThenInclude(c => c.Replies).FirstOrDefault();

            return new LoginUserInfoDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePic = user.ProfilePic,
                UserId = user.Id,
                IsPrivate = user.IsPrivate,
                Posts = user.Posts.OrderByDescending(p => p.Id).Select(p => new ShowPostDTO
                {
                    Comments = p.Comments.Select(c => new CommentDTO
                    {
                        Id = c.Id,
                        Text = c.Text,
                        FirstName = GetUserById(c.UserId).Result.FirstName,
                        LastName = GetUserById(c.UserId).Result.LastName,
                        PostId = c.PostId,
                        UserId = GetUserById(c.UserId).Result.Id,
                        ProfilePic = GetUserById(c.UserId).Result.ProfilePic
                    }).Take(3),
                    FileName = p.FileName,
                    Id = p.Id,
                    PostText = p.PostText
                }).Take(10)
            };
        }

        public async Task<LoginUserInfoDTO> EditUser(long userId, LoginUserInfoDTO user)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            User CurrentUser = await repository.GetEntityById(userId);

            CurrentUser.FirstName = user.FirstName;
            CurrentUser.LastName = user.LastName;
            CurrentUser.About = user.About;
            CurrentUser.Address = user.Address;
            CurrentUser.PhoneNumber = user.PhoneNumber;
            CurrentUser.BirthDay = DateTime.Parse(user.BirthDay);
            CurrentUser.UserGender = user.Gender;
            CurrentUser.SocialNetwork = user.SocialNetwork;
            CurrentUser.IsPrivate = user.IsPrivate;

            repository.UpdateEntity(CurrentUser);

            await unitOfWork.SaveChanges();

            return user;
        }

        public async Task ChangeUserSecurityDetails(long userId, ChangeUserSecutiryInfoDTO userSecurity)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();
            User user = await repository.GetEntityById(userId);

            if (userSecurity.Password != null && userSecurity.Password == userSecurity.RePassword)
                user.Password = PasswordHelper.EncodePasswordMd5(userSecurity.Password);

            if (user.IsPrivate != userSecurity.isPrivate)
                user.IsPrivate = userSecurity.isPrivate;


            repository.UpdateEntity(user);

            await unitOfWork.SaveChanges();
        }

        #endregion

        #region Notifications 
        public async Task<List<NotificationDTO>> GetNotificationsOfUser(long userId)//2
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();
            var notificationRepository = await unitOfWork.GetRepository<GenericRepository<Notification>, Notification>();


            return notificationRepository.GetEntitiesQuery().Where(n => (n.UserDestinationId == userId || n.UserOriginId == userId) && !n.IsDelete)
                .Select(n =>
                    new NotificationDTO
                    {
                        Id = n.Id,
                        UserOriginId = n.UserOriginId,
                        UserDestinationId = n.UserDestinationId,
                        IsRead = n.IsRead,
                        IsAccepted = n.IsAccepted,
                        TypeOfNotification = n.TypeOfNotification,
                        User = new LoginUserInfoDTO
                        {
                            UserId = n.UserOriginId,
                            FirstName = repository.GetEntitiesQuery().FirstOrDefault(u => u.Id == n.UserOriginId).FirstName,
                            LastName = repository.GetEntitiesQuery().FirstOrDefault(u => u.Id == n.UserOriginId).LastName,
                            ProfilePic = repository.GetEntitiesQuery().FirstOrDefault(u => u.Id == n.UserOriginId).ProfilePic,
                        },
                        CreateDate = n.CreateDate.Day.ToString() + "/" + n.CreateDate.Month.ToString() + "/" + n.CreateDate.Year.ToString()
                    }).ToList();
        }

        public async Task DeleteNotification(long notificationId)
        {
            var notificationRepository = await unitOfWork.GetRepository<GenericRepository<Notification>, Notification>();

            await notificationRepository.RemoveEntity(notificationId);

            await unitOfWork.SaveChanges();
        }

        public async Task<List<LoginUserInfoDTO>> GetLatestUsers()
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return repository.GetEntitiesQuery().OrderByDescending(u => u.CreateDate).Select(u => new LoginUserInfoDTO
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                ProfilePic = u.ProfilePic,
                UserId = u.Id
            }).Take(5).ToList();
        }

        public async Task DeleteRealTimeNotification(long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<RealTimeNotification>, RealTimeNotification>();

            var RealTimeNotificationByUserId = repository.GetEntitiesQuery().SingleOrDefault(u => u.UserId == userId);
            if (RealTimeNotificationByUserId != null)
            {
                repository.DeleteEntity(RealTimeNotificationByUserId);

                await unitOfWork.SaveChanges();
            }
        }

        public async Task AddRealTimeNotification(long userId, string connectionId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<RealTimeNotification>, RealTimeNotification>();

            await repository.AddEntity(new RealTimeNotification
            {
                UserId = userId,
                ConnectionId = connectionId
            });

            await unitOfWork.SaveChanges();
        }

        public async Task<string> GetConnectionIdByUserId(long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<RealTimeNotification>, RealTimeNotification>();
            if (repository.GetEntitiesQuery().Any(n => n.UserId == userId))
            {
                string connectionId = repository.GetEntitiesQuery().Single(n => n.UserId == userId).ConnectionId;
                if (connectionId != null)
                    return connectionId;

                return "";
            }
            else
                return "";
        }

        public async Task DeleteRealTimeNotification(RealTimeNotification notification)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<RealTimeNotification>, RealTimeNotification>();

            repository.DeleteEntity(notification);

            await unitOfWork.SaveChanges();
        }

    }

    #endregion
}

