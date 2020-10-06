using BBKBootcampSocial.Core.IServices;
using BBKBootcampSocial.DataLayer.Implementations;
using System;
using System.Threading.Tasks;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.Domains.User;

namespace BBKBootcampSocial.Core.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task AddUser(User user)
        {
            var Repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();
            await Repository.AddEntity(user);
            await unitOfWork.SaveChanges();

        }
    }
}
