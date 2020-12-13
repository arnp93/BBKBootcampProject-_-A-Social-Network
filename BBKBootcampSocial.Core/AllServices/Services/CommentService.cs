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

            if (comment.UserId != userId)
            {
                await notificationRepository.AddEntity(
                    new Notification
                    {
                        UserOriginId = userId,
                        UserDestinationId = await GetUserIdByPostId(comment.PostId),
                        PostId = comment.PostId,
                        IsRead = false,
                        IsAccepted = false,
                        IsDelete = false,
                        TypeOfNotification = TypeOfNotification.Comment
                    }
                );
            }
             
            Comment cm = mapper.Map<Comment>(comment);
            cm.UserId = userId;

            await repository.AddEntity(cm);

            await unitOfWork.SaveChanges();
            comment.Id = cm.Id;
            comment.DestinationUserId = await GetUserIdByPostId(comment.PostId);

            return comment;
        }


        public async Task<CommentReplyDTO> ReplyComment(CommentReplyDTO reply, long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Comment>, Comment>();
            var notificationRepository = await unitOfWork.GetRepository<GenericRepository<Notification>, Notification>();

            if (reply.UserId != userId)
            {
                await notificationRepository.AddEntity(
                    new Notification
                    {
                        UserOriginId = reply.UserId,
                        UserDestinationId = await GetUserIdByPostId(reply.PostId),
                        IsRead = false,
                        IsAccepted = false,
                        IsDelete = false,
                        TypeOfNotification = TypeOfNotification.Comment
                    }
                );
            }

            Comment cm = mapper.Map<Comment>(reply);

            await repository.AddEntity(cm);

            await unitOfWork.SaveChanges();
            reply.Id = cm.Id;
            reply.DestinationUserId = await GetUserIdByPostId(reply.PostId);

            return reply;
        }

        #endregion

        #region Tools

        public async Task<long> GetUserIdByPostId(long postId)
        {
            var postRepository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();
            var user = await postRepository.GetEntityById(postId);
            return user.UserId;
        }

        public async Task DeleteComment(long commentId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Comment>, Comment>();

            await repository.RemoveEntity(commentId);
        }


        #endregion

    }
}
