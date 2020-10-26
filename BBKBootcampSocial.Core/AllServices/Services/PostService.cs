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
using BBKBootcampSocial.Core.Paging;
using BBKBootcampSocial.Core.DTOs.Account;
using BBKBootcampSocial.Domains.Image;
using BBKBootcampSocial.Domains.User;
using Microsoft.AspNetCore.Http;

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
                .Include(p => p.Comments).ThenInclude(c => c.Replies).OrderByDescending(p => p.Id).Take(10).ToList();

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
                        ProfilePic = userService.GetUserById(c.UserId).Result.ProfilePic,
                        UserId = userService.GetUserById(c.UserId).Result.Id,
                        ParentId = c.ParentId,
                        Replies = c.Replies.Select(r => new CommentDTO
                        {
                            Id = r.Id,
                            FirstName = userService.GetUserById(r.UserId).Result.FirstName,
                            LastName = userService.GetUserById(r.UserId).Result.LastName,
                            Text = r.Text,
                            LikeCount = 0,
                            PostId = post.Id,
                            ProfilePic = userService.GetUserById(r.UserId).Result.ProfilePic,
                            UserId = userService.GetUserById(r.UserId).Result.Id,
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

        public async Task<List<Post>> LoadMorePosts(int currentPage, long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();
            BasePaging paging = Pager.Build(currentPage,10);
            return repository.GetEntitiesQuery().Where(p => p.UserId == userId).OrderByDescending(p => p.Id).Skip(paging.SkipPages).Take(paging.TakePages).Include(p => p.Comments).ThenInclude(c => c.Replies).ToList(); ;
        }

        public async Task<List<ShowPostDTO>> GetAllPosts()
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();

            List<ShowPostDTO> ShowPosts = new List<ShowPostDTO>();

            List<Post> posts = repository.GetEntitiesQuery().Include(p => p.User).Include(p => p.Comments).ThenInclude(c => c.Replies).OrderByDescending(p => p.Id).Take(10).ToList();

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
                        ProfilePic = userService.GetUserById(c.UserId).Result.ProfilePic,
                        UserId = userService.GetUserById(c.UserId).Result.Id,
                        ParentId = c.ParentId,
                        Replies = c.Replies.Select(r => new CommentDTO
                        {
                            Id = r.Id,
                            FirstName = userService.GetUserById(r.UserId).Result.FirstName,
                            LastName = userService.GetUserById(r.UserId).Result.LastName,
                            Text = r.Text,
                            LikeCount = 0,
                            PostId = post.Id,
                            ProfilePic = userService.GetUserById(r.UserId).Result.ProfilePic,
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
                    ParentId = null,
                    User = mapper.Map<LoginUserInfoDTO>(post.User)
                });
            }


            return ShowPosts;
        }

        public async Task<string> ProfilePic(IFormFile picture,long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();
            if (picture != null)
            {
                User user = await repository.GetEntityById(userId);
                if (user.ProfilePic != null)
                {
                  var imageRepository = await unitOfWork.GetRepository<GenericRepository<Image>, Image>();

                  await imageRepository.AddEntity(new Image
                  {
                      ImageName = user.ProfilePic,
                      UserId = user.Id
                  });
                }
                string picName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(picture.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProfilePictures", picName);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await picture.CopyToAsync(stream);
                user.ProfilePic = picName;
                await unitOfWork.SaveChanges();
                return picName;
            }
            return null;
        }

        #endregion

    }
}
