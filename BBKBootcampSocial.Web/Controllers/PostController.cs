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
        private readonly IPostService postService;
        public PostController(IPostService postService)
        {
            this.postService = postService;
        }
        public async Task<IActionResult> Post(PostDTO post)
        {
            if (!ModelState.IsValid)
                return JsonResponseStatus.Error();
            long userId = User.GetUserId();
            PostDTO savedPost = await postService.SavePost(userId,post);
            return JsonResponseStatus.Success(savedPost);
        }
    }
}
