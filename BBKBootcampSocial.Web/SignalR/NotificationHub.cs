using System.Linq;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.AllServices.IServices;
using BBKBootcampSocial.DataLayer.Implementations;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.Domains.User;
using Microsoft.AspNetCore.SignalR;

namespace BBKBootcampSocial.Web.SignalR
{
    public class NotificationHub : Hub
    {
        private readonly IUserService userService;
        private IUnitOfWork unitOfWork;
        public NotificationHub(IUserService userService, IUnitOfWork unitOfWork)
        {
            this.userService = userService;
            this.unitOfWork = unitOfWork;
        }
        public async Task SendMessage(string name = "Arash", string text = "Nabeghe")
        {
            await Clients.All.SendAsync("ClientFunc", name, text);
        }

        public async Task SaveConnection(long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<RealTimeNotification>, RealTimeNotification>();

            if (repository.GetEntitiesQuery().Any(uid => uid.UserId == userId))
            {
                var userRealTimeNotification = repository.GetEntitiesQuery().SingleOrDefault(uid => uid.UserId == userId);
                await userService.DeleteRealTimeNotification(userRealTimeNotification);
            }
            await repository.AddEntity(new RealTimeNotification
            {
                UserId = userId,
                ConnectionId = Context.ConnectionId
            });

            await unitOfWork.SaveChanges();
        }
    }
}
