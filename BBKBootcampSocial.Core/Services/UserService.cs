using System;
using BBKBootcampSocial.Core.IServices;
using BBKBootcampSocial.DataLayer.Implementations;
using System.Threading.Tasks;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.Domains.User;
using BBKBootcampSocial.Core.DTOs.Account;
using AutoMapper;
using System.Linq;

namespace BBKBootcampSocial.Core.Services
{
    public class UserService : IUserService
    {
        #region Constructor

        private IUnitOfWork unitOfWork;
        private IMapper mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        #endregion

        #region Register

        public async Task<RegisterUserResult> AddUser(RegisterUserDTO user)
        {
            var Repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();
            User User = mapper.Map<User>(user);
            if (await IsEmailExist(User.Email))
            {
                return RegisterUserResult.EmailExists;
            }
            await Repository.AddEntity(User);
            await unitOfWork.SaveChanges();
            return RegisterUserResult.Success;

        }
        #endregion

        #region Login

        public async Task<LoginUserResult> LoginUser(LoginUserDTO login)
        {
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

        #region Tools

        public async Task<bool> IsEmailExist(string email)
        {
            var Repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return Repository.GetEntitiesQuery().Any(u => u.Email == email);

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
            var Repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return Repository.GetEntitiesQuery().FirstOrDefault(u => u.Email == email);
        }

        public async Task<bool> IsUserActive(string email)
        {
            var Repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return Repository.GetEntitiesQuery().Any(u => u.Email == email && u.IsActive == true);
        }

        #endregion
    }
}
