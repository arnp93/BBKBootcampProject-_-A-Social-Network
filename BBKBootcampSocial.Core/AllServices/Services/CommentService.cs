using BBKBootcampSocial.Core.AllServices.IServices;
using BBKBootcampSocial.Core.DTOs.Comment;
using System.Threading.Tasks;
using AutoMapper;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.DataLayer.Implementations;
using BBKBootcampSocial.Domains.Comment;
using BBKBootcampSocial.Domains.Common_Entities;
using BBKBootcampSocial.Domains.User;
using BBKBootcampSocial.Domains.Post;

namespace BBKBootcampSocial.Core.AllServices.Services
{
    public class CommentService : ICommentService
    {
        #region Constructor

        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        private IUserService userService;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userService = userService;
        }

        #endregion

        #region Properties

        public async Task<NewCommentDTO> AddComment(NewCommentDTO comment,long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Comment>, Comment>();
            var notificationRepository = await unitOfWork.GetRepository<GenericRepository<Notification>, Notification>();

                await notificationRepository.AddEntity(
                    new Notification
                    {
                        UserOriginId = comment.UserId,
                        UserDestinationId = await GetUserIdByPostId(comment.PostId),
                        IsRead = false,
                        IsAccepted=false,
                        IsDelete = false,
                        TypeOfNotification = TypeOfNotification.Comment
                    }
                );
      
        


            Comment cm = mapper.Map<Comment>(comment);

            await repository.AddEntity(cm);

            await unitOfWork.SaveChanges();
            comment.Id = cm.Id;

            return comment;
        }

        #endregion

        #region Tools

        public async Task<long> GetUserIdByPostId(long postId)
        {
            var postRepository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();

            return postRepository.GetEntityById(postId).Result.UserId;
        }

        #endregion

    }
}
