using System.Collections.Generic;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.DTOs.Post;
using BBKBootcampSocial.Domains.Post;
using Microsoft.AspNetCore.Http;

namespace BBKBootcampSocial.Core.AllServices.IServices
{
    public interface IPostService
    {
        Task<ShowPostDTO> SavePost(long userId, PostDTO post);
        Task<bool> DeletePost(long postId);
        Task<bool> DeletePost(PostDTO post);
        Task<PostDTO> EditPost(PostDTO post);
        Task<List<ShowPostDTO>> PostsOfUser(long userId);
        Task<List<Post>> LoadMorePosts(int currentPage, long userId);
        Task<List<ShowPostDTO>> GetAllPosts();
        Task<string> ProfilePic(IFormFile picture,long userId);
    }
}
