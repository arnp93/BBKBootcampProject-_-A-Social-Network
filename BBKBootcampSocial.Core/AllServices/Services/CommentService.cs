using BBKBootcampSocial.Core.AllServices.IServices;
using BBKBootcampSocial.Core.DTOs.Comment;
using System.Threading.Tasks;
using AutoMapper;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.DataLayer.Implementations;
using BBKBootcampSocial.Domains.Comment;

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

        public async Task<NewCommentDTO> AddComment(NewCommentDTO comment)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Comment>, Comment>();

            Comment cm = mapper.Map<Comment>(comment);

            await repository.AddEntity(cm);

            await unitOfWork.SaveChanges();
            comment.Id = cm.Id;

            return comment;
        }

        #endregion

    }
}
