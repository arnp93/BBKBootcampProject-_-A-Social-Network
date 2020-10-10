using System.Threading.Tasks;
using BBKBootcampSocial.Core.DTOs.Post;

namespace BBKBootcampSocial.Core.IServices
{
    public interface IPostService
    {
        Task<PostDTO> SavePost(long userId, PostDTO post);
        Task<bool> DeletePost(long postId);
        Task<bool> DeletePost(PostDTO post);
        Task<PostDTO> EditPost(PostDTO post);
    }
}
