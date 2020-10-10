using System;
using BBKBootcampSocial.Core.IServices;
using BBKBootcampSocial.DataLayer.Implementations;
using System.Threading.Tasks;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.Domains.User;
using BBKBootcampSocial.Core.DTOs.Account;
using AutoMapper;
using System.Linq;
using BBKBootcampSocial.Core.Security;
using BBKBootcampSocial.Core.Utilities.Convertors;

namespace BBKBootcampSocial.Core.Services
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

            user.Email = user.Email.SanitizeText();
            user.FirstName = user.FirstName.SanitizeText();
            user.LastName = user.LastName.SanitizeText();
            user.Password = user.Password.SanitizeText();
            user.RePassword = user.RePassword.SanitizeText();

            #endregion

            User User = mapper.Map<User>(user);
            User.Password = PasswordHelper.EncodePasswordMd5(User.Password);
            if (await IsEmailExist(User.Email))
            {
                return RegisterUserResult.EmailExists;
            }

            User.ActiveCode = Guid.NewGuid().ToString() + "BBK-BootCamp";
            await repository.AddEntity(User);
            await unitOfWork.SaveChanges();


            string body = await viewRenderService.RenderToStringAsync("Email/ActivateAccount", User);
            mailSender.Send(user.Email, "BBK Bootcamp Social Network - Activate your Account", body);
            return RegisterUserResult.Success;

        }

        #endregion

        #region Login

        public async Task<LoginUserResult> LoginUser(LoginUserDTO login)
        {
            login.Password = PasswordHelper.EncodePasswordMd5(login.Password);
            try
            {
                if (!await IsEmailExist(login.Email))
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
                if (password == user.Password)
                    return true;
                else
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

            return repository.GetEntitiesQuery().Any(u => u.Email == email && u.IsActive == true);
        }

        public async Task<User> GetUserById(long id)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return repository.GetEntitiesQuery().FirstOrDefault(u => u.Id == id);
        }

        #endregion
    }
}
