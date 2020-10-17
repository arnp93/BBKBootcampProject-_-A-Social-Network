using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.Domains.Post;
using BBKBootcampSocial.DataLayer.Implementations;
using BBKBootcampSocial.Core.DTOs.Post;
using System.Collections.Generic;
using System.Linq;
using BBKBootcampSocial.Core.AllServices.IServices;
using Microsoft.EntityFrameworkCore;
using BBKBootcampSocial.Core.DTOs.Comment;

namespace BBKBootcampSocial.Core.AllServices.Services
{
    public class PostService : IPostService
    {
        #region Constructor

        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        private IUserService userService;
        public PostService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userService = userService;
        }

        #endregion

        #region Properties

        public async Task<ShowPostDTO> SavePost(long userId, PostDTO post)
        {
            post.UserId = userId;
            var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();

            Post savedPost = mapper.Map<Post>(post);
            savedPost.TimesOfReports = 0;
            if (post.FileName != null)
            {
                savedPost.FileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(post.FileName.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PostFiles", savedPost.FileName);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await post.FileName.CopyToAsync(stream);
            }
            await repository.AddEntity(savedPost);
            await unitOfWork.SaveChanges();
            return new ShowPostDTO
            {
                PostText = savedPost.PostText,
                FileName = savedPost.FileName,
                DateTime = savedPost.CreateDate,
                Id = savedPost.Id
            };
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

        public async Task<List<ShowPostDTO>> PostsOfUser(long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();

            List<ShowPostDTO> ShowPosts = new List<ShowPostDTO>();

            List<Post> posts = repository.GetEntitiesQuery().Where(p => p.UserId == userId)
                .Include(p => p.Comments).ThenInclude(c => c.Replies).ToList();

            foreach (var post in posts)
            {
                ShowPosts.Add(new ShowPostDTO
                {
                    Comments = post.Comments.Select(c => new CommentDTO
                    {
                        Id = c.Id,
                        FirstName = userService.GetUserById(c.UserId).Result.FirstName,
                        LastName = userService.GetUserById(c.UserId).Result.LastName,
                        Text = c.Text,
                        LikeCount = 0,
                        PostId = post.Id,
                        ProfileImage = null,
                        UserId = post.UserId,
                        ParentId = c.ParentId,
                        Replies = c.Replies.Select(r => new CommentDTO
                        {
                            Id = r.Id,
                            FirstName = userService.GetUserById(r.UserId).Result.FirstName,
                            LastName = userService.GetUserById(r.UserId).Result.LastName,
                            Text = r.Text,
                            LikeCount = 0,
                            PostId = post.Id,
                            ProfileImage = null,
                            UserId = post.UserId,
                            ParentId = r.ParentId
                        }).Take(2)
                    }).Take(3),
                    PostText = post.PostText,
                    DateTime = post.CreateDate,
                    FileName = post.FileName,
                    Id = post.Id,
                    UserId = post.UserId,
                    CanalId = null,
                    ParentId = null
                });
            }
            return ShowPosts;

            //List<Post> posts = repository.GetEntitiesQuery().Where(p => p.UserId == userId)
            //    .Include(p => p.Comments.Select(c => new CommentDTO
            //    {
            //        FirstName = userService.GetUserById(c.UserId).Result.FirstName.ToString(),
            //        LastName = userService.GetUserById(c.UserId).Result.LastName.ToString(),
            //        Text = c.Text
            //    })).ToList();

            //return mapper.Map<List<ShowPostDTO>>(posts);
        }

        #endregion

    }
}
