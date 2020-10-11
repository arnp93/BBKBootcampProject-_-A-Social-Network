using BBKBootcampSocial.Core.IServices;
using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.Domains.Post;
using BBKBootcampSocial.DataLayer.Implementations;
using BBKBootcampSocial.Core.DTOs.Post;

namespace BBKBootcampSocial.Core.Services
{
    public class PostService : IPostService
    {
        #region Constructor

        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        #endregion

        #region Properties

        public async Task<PostDTO> SavePost(long userId, PostDTO post)
        {
            post.UserId = userId;
            var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();

            Post savedPost = mapper.Map<Post>(post);
            if (post.FileName != null)
            {
                savedPost.FileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(post.FileName.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PostFiles", savedPost.FileName);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await post.FileName.CopyToAsync(stream);
            }
            await repository.AddEntity(savedPost);

            return post;
        }

        public async Task<bool> DeletePost(long postId)
        {
            try
            {
                var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();
                await repository.RemoveEntity(postId);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public async Task<bool> DeletePost(PostDTO post)
        {
            try
            {
                var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();
                Post deletedPost = mapper.Map<Post>(post);
                repository.RemoveEntity(deletedPost);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public async Task<PostDTO> EditPost(PostDTO post)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();
            Post editedPost = mapper.Map<Post>(post);
            repository.UpdateEntity(editedPost);

            return post;
        }

        #endregion

    }
}
