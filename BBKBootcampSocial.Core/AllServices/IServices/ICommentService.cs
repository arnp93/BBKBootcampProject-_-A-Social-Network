using System.Threading.Tasks;
using BBKBootcampSocial.Core.DTOs.Comment;

namespace BBKBootcampSocial.Core.AllServices.IServices
{
    public interface ICommentService
    {
        Task<NewCommentDTO> AddComment(NewCommentDTO comment, long userId);
        Task<CommentReplyDTO> ReplyComment(CommentReplyDTO reply, long userId);
        Task DeleteComment(long commentId);
        Task<long> GetUserIdByPostId(long postId);
    }
}
