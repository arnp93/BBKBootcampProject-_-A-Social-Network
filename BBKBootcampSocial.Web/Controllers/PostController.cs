using BBKBootcampSocial.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.AllServices.IServices;
using BBKBootcampSocial.Core.DTOs.Post;
using BBKBootcampSocial.Core.Utilities.Identity;
using Microsoft.AspNetCore.Authorization;
using BBKBootcampSocial.Core.DTOs.Comment;

namespace BBKBootcampSocial.Web.Controllers
{
    //[Authorize]
    public class PostController : BaseController
    {
        #region Constructor

        private readonly IPostService postService;
        private readonly ICommentService commentService;
        public PostController(IPostService postService, ICommentService commentService)
        {
            this.postService = postService;
            this.commentService = commentService;
        }

        #endregion

        #region Posts Properties

        [HttpPost("new-post")]
        public async Task<IActionResult> Post([FromForm] PostDTO post)
        {
            if (!ModelState.IsValid)
                return JsonResponseStatus.Error();
            long userId = User.GetUserId();
            PostDTO savedPost = await postService.SavePost(userId, post);
            return JsonResponseStatus.Success(savedPost);
        }
        [HttpGet("user-posts")]
        public async Task<IActionResult> GetUserPosts()
        {
            //long userId = User.GetUserId();

            return JsonResponseStatus.Success(await postService.PostsOfUser(10032));
        }

        #endregion

        #region Comments Properties

        [HttpPost("new-comment")]
        public async Task<IActionResult> PostComment(NewCommentDTO comment)
        {
            if (!ModelState.IsValid)
                return JsonResponseStatus.Error();
            long userId = User.GetUserId();
            await commentService.AddComment(new NewCommentDTO {Text = comment.Text, PostId = comment.PostId,UserId = userId });
            return JsonResponseStatus.Success();
        }

        #endregion

    }
}
