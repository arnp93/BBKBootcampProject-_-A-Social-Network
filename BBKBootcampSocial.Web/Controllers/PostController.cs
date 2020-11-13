using BBKBootcampSocial.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.AllServices.IServices;
using BBKBootcampSocial.Core.DTOs.Post;
using BBKBootcampSocial.Core.Utilities.Identity;
using Microsoft.AspNetCore.Authorization;
using BBKBootcampSocial.Core.DTOs.Comment;
using BBKBootcampSocial.Domains.User;
using BBKBootcampSocial.Web.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using BBKBootcampSocial.Core.DTOs.Notification;
using BBKBootcampSocial.Domains.Common_Entities;
using BBKBootcampSocial.Core.DTOs.Account;
using System;

namespace BBKBootcampSocial.Web.Controllers
{
    [Authorize]
    public class PostController : BaseController
    {
        #region Constructor

        private readonly IPostService postService;
        private readonly ICommentService commentService;
        private readonly IUserService userService;
        private readonly IHubContext<NotificationHub> hubContext;

        public PostController(IPostService postService, ICommentService commentService, IUserService userService, IHubContext<NotificationHub> hubContext)
        {
            this.postService = postService;
            this.commentService = commentService;
            this.userService = userService;
            this.hubContext = hubContext;
        }

        #endregion

        #region Posts Properties

        [HttpPost("new-post")]
        public async Task<IActionResult> Post([FromForm] PostDTO post)
        {
            if (!ModelState.IsValid)
                return JsonResponseStatus.Error();
            long userId = User.GetUserId();
            ShowPostDTO savedPost = await postService.SavePost(userId, post);
            return JsonResponseStatus.Success(savedPost);
        }

        [HttpGet("user-posts")]
        public async Task<IActionResult> GetUserPosts()
        {
            long userId = User.GetUserId();

            return JsonResponseStatus.Success(await postService.PostsOfUser(userId));
        }

        [HttpGet("all-posts")]
        public async Task<IActionResult> GetAllPosts()
        {
            return JsonResponseStatus.Success(await postService.GetAllPosts());
        }

        [HttpPost("edit-post")]
        public async Task<IActionResult> EditPost([FromBody] EditPostDTO newPost)
        {
            if (ModelState.IsValid)
            {
                await postService.EditPost(newPost);
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();

        }

        [HttpPost("delete-post")]
        public async Task<IActionResult> DeletePost([FromBody] long postId)
        {
            if (await postService.DeletePost(postId))
            {
                return JsonResponseStatus.Success();
            }
            return JsonResponseStatus.Error();
        }

        [HttpGet("friends-posts")]
        public async Task<IActionResult> GetFriendsPosts()
        {
            long userId = User.GetUserId();
            return JsonResponseStatus.Success(await postService.GetFriendsPosts(userId));
        }

        [HttpPost("get-single-post")]
        public async Task<IActionResult> GetSoinglePost([FromBody] long postId)
        {
            return JsonResponseStatus.Success(await postService.GetPostById(postId));
        }

        #endregion

        #region Paging

        [HttpPost("load-posts")]
        public async Task<IActionResult> LoadMorePosts([FromForm] int currentPage)
        {
            long userId = User.GetUserId();

            return JsonResponseStatus.Success(await postService.LoadMorePosts(currentPage, userId));
        }

        [HttpPost("load-newsfeed-posts")]
        public async Task<IActionResult> LoadMoreNewsfeedPosts([FromForm] int currentPage)
        {
            return JsonResponseStatus.Success(await postService.LoadMorePosts(currentPage));
        }

        #endregion

        #region Comments Properties

        [HttpPost("new-comment")]
        public async Task<IActionResult> PostComment(NewCommentDTO comment)
        {
            if (!ModelState.IsValid)
                return JsonResponseStatus.Error();
            long userId = User.GetUserId();
            NewCommentDTO newComment = await commentService.AddComment(new NewCommentDTO {Text = comment.Text, PostId = comment.PostId, UserId = comment.UserId },userId);
            User user = userService.GetUserById(userId).Result;
      
            string connectionId = await userService.GetConnectionIdByUserId(newComment.DestinationUserId);
            if (connectionId != "" && userId != newComment.DestinationUserId)
            {
                await hubContext.Clients.Client(connectionId).SendAsync("NewComment", new NotificationDTO
                {
                    Id = newComment.Id,
                    UserDestinationId = newComment.DestinationUserId,
                    PostId = newComment.PostId,
                    User = new LoginUserInfoDTO
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        ProfilePic = user.ProfilePic,
                    },
                    CreateDate = DateTime.Now.ToString(),
                    Message =" left a comment for you",
                    UserOriginId =user.Id,
                    TypeOfNotification = TypeOfNotification.Comment,
                    IsAccepted = false,
                    IsRead = false
                });
            }

            return JsonResponseStatus.Success(new CommentDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ParentId = comment.PostId,
                Text = comment.Text,
                UserId = user.Id,
                Id = newComment.Id,
                ProfilePic = user.ProfilePic,
                PostId = comment.PostId
            });
        }

        [HttpPost("reply-comment")]
        public async Task<IActionResult> ReplyComment(CommentReplyDTO comment)
        {
            if (!ModelState.IsValid)
                return JsonResponseStatus.Error();

            long userId = User.GetUserId();
            CommentReplyDTO newComment = await commentService.ReplyComment(new CommentReplyDTO { Text = comment.Text, PostId = comment.PostId,ParentId = comment.ParentId, UserId = userId }, userId);
            User user = userService.GetUserById(userId).Result;

            string connectionId = await userService.GetConnectionIdByUserId(newComment.DestinationUserId);
            if (connectionId != "" && userId != newComment.DestinationUserId)
            {
                await hubContext.Clients.Client(connectionId).SendAsync("NewComment", new NotificationDTO
                {
                    Id = newComment.Id,
                    UserDestinationId = newComment.DestinationUserId,
                    PostId = newComment.PostId,
                    User = new LoginUserInfoDTO
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        ProfilePic = user.ProfilePic,
                    },
                    CreateDate = DateTime.Now.ToString(),
                    Message = " left a comment for you",
                    UserOriginId = user.Id,
                    TypeOfNotification = TypeOfNotification.Comment,
                    IsAccepted = false,
                    IsRead = false
                });
            }

            return JsonResponseStatus.Success(new CommentDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ParentId = comment.ParentId,
                Text = comment.Text,
                UserId = user.Id,
                Id = newComment.Id,
                ProfilePic = user.ProfilePic,
                PostId = comment.PostId
            });
        }

        #endregion

        #region Add/Change Profile Picture / Cover Picture
        [HttpPost("profile-pic")]
        public async Task<IActionResult> ProfilePicture([FromForm] IFormFile pic)
        {
            long userId = User.GetUserId();
            string pictureName = await postService.ProfilePic(pic,userId);
            if (pictureName != null)
                return JsonResponseStatus.Success(pictureName);
            else
                return JsonResponseStatus.Error();
        }

        [HttpPost("cover-pic")]
        public async Task<IActionResult> CoverPicture([FromForm] IFormFile pic)
        {
            long userId = User.GetUserId();
            string pictureName = await postService.CoverPic(pic, userId);
            if (pictureName != null)
                return JsonResponseStatus.Success(pictureName);
            else
                return JsonResponseStatus.Error();
        }

        #endregion

        #region Likes Section

        [HttpPost("like")]
        public async Task<IActionResult> AddOrRemoveLike([FromBody]long postId)
        {
            long userId = User.GetUserId();
            var like = await postService.AddOrRemoveLike(postId, userId);
            return JsonResponseStatus.Success(like);
        }

        #endregion

        #region Posts With Hashtag

        [HttpPost("hashtag-posts")]
        public async Task<IActionResult> GetPostsWithHashtag([FromForm]string hashtagText)
        {
            return JsonResponseStatus.Success(await postService.PostsWithHashtag(hashtagText));
        }

        #endregion
    }
}
