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
        private IUnitOfWork unitOfWork;
        private IMapper mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<RegisterUserResult> AddUser(RegisterUserDTO user)
        {
            var Repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();
            User User = mapper.Map<User>(user);
            if (await isEmailExist(User.Email))
            {
                return RegisterUserResult.EmailExists;
            }
            await Repository.AddEntity(User);
            await unitOfWork.SaveChanges();
            return RegisterUserResult.Success;

        }

        public async Task<bool> isEmailExist(string email)
        {
            var Repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();

            return Repository.GetEntitiesQuery().Any(u => u.Email == email);

        }
    }
}
