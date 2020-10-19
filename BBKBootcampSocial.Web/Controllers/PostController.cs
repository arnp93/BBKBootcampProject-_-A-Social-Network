using BBKBootcampSocial.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.AllServices.IServices;
using BBKBootcampSocial.Core.DTOs.Post;
using BBKBootcampSocial.Core.Utilities.Identity;
using Microsoft.AspNetCore.Authorization;
using BBKBootcampSocial.Core.DTOs.Comment;
using BBKBootcampSocial.Core.Paging;
using BBKBootcampSocial.Domains.User;
using Microsoft.AspNetCore.Http;

namespace BBKBootcampSocial.Web.Controllers
{
    //[Authorize]
    public class PostController : BaseController
    {
        #region Constructor

        private readonly IPostService postService;
        private readonly ICommentService commentService;
        private readonly IUserService userService;

        public PostController(IPostService postService, ICommentService commentService, IUserService userService)
        {
            this.postService = postService;
            this.commentService = commentService;
            this.userService = userService;
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

        [HttpPost("load-posts")]
        public async Task<IActionResult> LoadMorePosts([FromForm]int currentPage)
        {
            long userId = User.GetUserId();

            return JsonResponseStatus.Success(await postService.LoadMorePosts(currentPage,userId));
        }
        #endregion

        #region Comments Properties

        [HttpPost("new-comment")]
        public async Task<IActionResult> PostComment(NewCommentDTO comment)
        {
            if (!ModelState.IsValid)
                return JsonResponseStatus.Error();
            long userId = User.GetUserId();
            NewCommentDTO newComment = await commentService.AddComment(new NewCommentDTO {Text = comment.Text, PostId = comment.PostId,UserId = userId });
            User user = userService.GetUserById(userId).Result;
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

        #endregion

        #region Add/Change Profile Picture
        [HttpPost("profile-pic")]
        public async Task<IActionResult> ProfilePicture([FromForm] IFormFile pic)
        {
            long userId = User.GetUserId();
            string name = await postService.ProfilePic(pic,userId);
            if (name != null)
                return JsonResponseStatus.Success(name);
            else
                return JsonResponseStatus.Error();
        }

        #endregion

    }
}
