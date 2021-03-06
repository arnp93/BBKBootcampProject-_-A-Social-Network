﻿using System.Collections.Generic;

namespace BBKBootcampSocial.Core.DTOs.Comment
{
    public class CommentDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePic { get; set; }
        public long UserId { get; set; }
        public string Text { get; set; }
        public int LikeCount { get; set; }
        public long Id { get; set; }
        public long PostId { get; set; }
        public long? ParentId { get; set; }
        public IEnumerable<CommentDTO> Replies { get; set; }
    }
}
