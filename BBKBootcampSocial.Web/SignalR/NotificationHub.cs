using System.Linq;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.AllServices.IServices;
using BBKBootcampSocial.Core.DTOs.Notification;
using BBKBootcampSocial.DataLayer.Implementations;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.Domains.Common_Entities;
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

        public async Task AddFriendNotification(long userId,NotificationDTO notification)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<RealTimeNotification>, RealTimeNotification>();

            var userRealTimeNotification = repository.GetEntitiesQuery().SingleOrDefault(uid => uid.UserId == userId);

            if (userRealTimeNotification != null)
            {
                var newNotification = new NotificationDTO
                {
                    Id = notification.Id,
                    UserDestinationId = notification.UserDestinationId,
                    UserOriginId = notification.UserOriginId,
                    TypeOfNotification = TypeOfNotification.FriendRequest,
                    IsAccepted = notification.IsAccepted,
                    IsRead = notification.IsRead
                };
                await Clients.Client(userRealTimeNotification.ConnectionId).SendAsync("AddFriendRequest", newNotification);
            }
        }
    }
}
