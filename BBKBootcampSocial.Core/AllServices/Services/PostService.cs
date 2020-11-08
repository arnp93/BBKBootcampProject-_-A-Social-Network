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

        #region Post Properties

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
                        }).OrderByDescending(r => r.Id).Take(2)
                    }).OrderByDescending(c => c.Id).Take(3),
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
            BasePaging paging = Pager.Build(currentPage, 10);
            return repository.GetEntitiesQuery().Where(p => p.UserId == userId).OrderByDescending(p => p.Id).Skip(paging.SkipPages).Take(paging.TakePages).Include(p => p.Comments).ThenInclude(c => c.Replies).ToList();
        }
        public async Task<List<Post>> LoadMorePosts(int currentPage)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();
            BasePaging paging = Pager.Build(currentPage, 10);
            return repository.GetEntitiesQuery().OrderByDescending(p => p.Id).Skip(paging.SkipPages).Take(paging.TakePages).Include(p => p.User).Include(p => p.Comments).ThenInclude(c => c.Replies).ToList();
        }

        public async Task<List<ShowPostDTO>> GetAllPosts()
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();

            List<ShowPostDTO> ShowPosts = new List<ShowPostDTO>();

            List<Post> posts = repository.GetEntitiesQuery().Include(p => p.User).Include(p => p.Likes).Include(p => p.Comments).ThenInclude(c => c.Replies).OrderByDescending(p => p.Id).Take(10).ToList();

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
                        }).OrderByDescending(r => r.Id).Take(2)
                    }).OrderByDescending(c => c.Id).Take(3),
                    Likes = post.Likes.Where(l => !l.IsDelete).Select(pl => new LikeDTO
                    {
                        Id = pl.Id,
                        UserId = pl.UserId,
                        FirstName = userService.GetUserById(pl.UserId).Result.FirstName,
                        LastName = userService.GetUserById(pl.UserId).Result.LastName,
                        ProfilePic = userService.GetUserById(pl.UserId).Result.ProfilePic
                    }).ToList(),
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

        public async Task<string> ProfilePic(IFormFile picture, long userId)
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
                string pictureName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(picture.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProfilePictures", pictureName);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await picture.CopyToAsync(stream);
                user.ProfilePic = pictureName;
                repository.UpdateEntity(user);
                await unitOfWork.SaveChanges();
                return pictureName;
            }
            return null;
        }

        public async Task<string> CoverPic(IFormFile picture, long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<User>, User>();
            if (picture != null)
            {
                User user = await repository.GetEntityById(userId);
                if (user.CoverPic != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CoverPictures", user.CoverPic);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                }
                string pictureName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(picture.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CoverPictures", pictureName);
                await using var stream = new FileStream(imagePath, FileMode.Create);
                await picture.CopyToAsync(stream);
                user.CoverPic = pictureName;
                repository.UpdateEntity(user);
                await unitOfWork.SaveChanges();
                return pictureName;
            }
            return null;
        }

        public async Task EditPost(EditPostDTO editPost)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();

            var post = await repository.GetEntityById(editPost.PostId);

            post.PostText = editPost.PostText;

            repository.UpdateEntity(post);

            await unitOfWork.SaveChanges();
        }

        public async Task<List<ShowPostDTO>> GetFriendsPosts(long userId)
        {
            var friendRepository = await unitOfWork.GetRepository<GenericRepository<UserFriend>, UserFriend>();
            var postRepository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();

            List<ShowPostDTO> showPosts = new List<ShowPostDTO>();

            var friendsIds = friendRepository.GetEntitiesQuery().Where(f => f.UserId == userId)
                .Select(f => f.FriendUserId).Take(10).ToList();
            var posts = postRepository.GetEntitiesQuery().Where(p => friendsIds.Contains(p.UserId)).Include(p => p.User).Include(p => p.Comments).ThenInclude(c => c.Replies).Include(p => p.Likes).ToList();


            foreach (var post in posts)
            {

                showPosts.Add(new ShowPostDTO
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
                    Likes = post.Likes.Where(l => !l.IsDelete).Select(pl => new LikeDTO
                    {
                        Id = pl.Id,
                        UserId = pl.UserId,
                        FirstName = userService.GetUserById(pl.UserId).Result.FirstName,
                        LastName = userService.GetUserById(pl.UserId).Result.LastName,
                        ProfilePic = userService.GetUserById(pl.UserId).Result.ProfilePic
                    }).ToList(),
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

            return showPosts;
        }


        #endregion

        #region Like Properties
        public async Task<LikeDTO> AddOrRemoveLike(long postId, long userId)
        {
            var repository = await unitOfWork.GetRepository<GenericRepository<Like>, Like>();
            var postRepository = await unitOfWork.GetRepository<GenericRepository<Post>, Post>();

            Post post = postRepository.GetEntitiesQuery().Where(p => p.Id == postId).Include(p => p.Likes).FirstOrDefault();

            if (post != null && !post.IsDelete)
            {
                if (post.Likes.Any(l => l.UserId == userId && !l.IsDelete))
                {
                    Like like = repository.GetEntitiesQuery().Single(l => l.UserId == userId && l.PostId == postId);

                    repository.RemoveEntity(like);
                    await unitOfWork.SaveChanges();
                    return new LikeDTO
                    {
                        Id = like.Id,
                        UserId = userId,
                        FirstName = userService.GetUserById(userId).Result.FirstName,
                        LastName = userService.GetUserById(userId).Result.LastName,
                        ProfilePic = userService.GetUserById(userId).Result.ProfilePic
                    }; 
                }
                if (post.Likes.Any(l => l.UserId == userId && l.IsDelete))
                {
                    Like like = repository.GetEntitiesQuery().Single(l => l.UserId == userId && l.PostId == postId);
                    like.IsDelete = false;
                    repository.UpdateEntity(like);
                    await unitOfWork.SaveChanges();
                    return new LikeDTO
                    {
                        Id = like.Id,
                        UserId = userId,
                        FirstName = userService.GetUserById(userId).Result.FirstName,
                        LastName = userService.GetUserById(userId).Result.LastName,
                        ProfilePic = userService.GetUserById(userId).Result.ProfilePic
                    };
                }

                await repository.AddEntity(new Like
                    {
                        UserId = userId,
                        PostId = postId
                    });
                
              

                await unitOfWork.SaveChanges();

                return new LikeDTO
                {
                    UserId = userId,
                    FirstName = userService.GetUserById(userId).Result.FirstName,
                    LastName = userService.GetUserById(userId).Result.LastName,
                    ProfilePic = userService.GetUserById(userId).Result.ProfilePic
                };
            }

            return null;
        }

        #endregion

    }
}
