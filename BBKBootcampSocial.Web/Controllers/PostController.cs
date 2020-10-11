using System.Collections.Generic;
using BBKBootcampSocial.Core.IServices;
using BBKBootcampSocial.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.DTOs.Post;
using BBKBootcampSocial.Core.Utilities.Identity;

namespace BBKBootcampSocial.Web.Controllers
{
    public class PostController : BaseController
    {
        #region Constructor

        private readonly IPostService postService;
        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        #endregion

        #region Properties

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
            long userId = User.GetUserId();

            return JsonResponseStatus.Success(await postService.PostsOfUser(userId));
        }

        #endregion

    }
}
