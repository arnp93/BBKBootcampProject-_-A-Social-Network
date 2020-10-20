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
                RoleId = 2,
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
                if (!await IsUserExist(login.Email,login.Password))
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
                Posts = user.Posts.Select(p => new ShowPostDTO
                {
                    Comments = p .Comments.Select(c => new CommentDTO
                    {
                        Id = c.Id,
                        Text = c.Text,
                        FirstName = GetUserById(c.UserId).Result.FirstName,
                        LastName = GetUserById(c.UserId).Result.LastName,
                        PostId = c.PostId
                    }),
                    FileName = p.FileName,
                    Id = p.Id,
                    PostText = p.PostText
                })
            };
        }

        #endregion
    }
}
