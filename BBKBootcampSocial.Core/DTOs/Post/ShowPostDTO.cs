using BBKBootcampSocial.Core.DTOs.Account;
using BBKBootcampSocial.Core.DTOs.Comment;
using System;
using System.Collections.Generic;

namespace BBKBootcampSocial.Core.DTOs.Post
{
    public class ShowPostDTO
    {
        public string PostText { get; set; }
        public string FileName { get; set; }
        public long UserId { get; set; }
        public long? CanalId { get; set; }
        public DateTime DateTime { get; set; }
        public long? ParentId { get; set; }
        public long Id { get; set; }


        public LoginUserInfoDTO User { get; set; }

        public IEnumerable<CommentDTO> Comments{ get; set; }
        public IEnumerable<LikeDTO> Likes{ get; set; }
    }
}
