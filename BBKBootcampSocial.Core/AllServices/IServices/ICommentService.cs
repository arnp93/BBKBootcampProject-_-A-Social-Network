using System.Threading.Tasks;
using BBKBootcampSocial.Core.DTOs.Comment;

namespace BBKBootcampSocial.Core.AllServices.IServices
{
    public interface ICommentService
    {
        Task<NewCommentDTO> AddComment(NewCommentDTO comment);
    }
}
