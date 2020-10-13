using System.Collections.Generic;
using System.Threading.Tasks;
using BBKBootcampSocial.Core.DTOs.Post;

namespace BBKBootcampSocial.Core.AllServices.IServices
{
    public interface IPostService
    {
        Task<PostDTO> SavePost(long userId, PostDTO post);
        Task<bool> DeletePost(long postId);
        Task<bool> DeletePost(PostDTO post);
        Task<PostDTO> EditPost(PostDTO post);
        Task<List<ShowPostDTO>> PostsOfUser(long userId);
    }
}
