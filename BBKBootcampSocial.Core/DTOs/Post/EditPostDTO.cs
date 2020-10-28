using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BBKBootcampSocial.Core.DTOs.Post
{
    public class EditPostDTO
    {
        [Required]
        public long PostId { get; set; }
        public string PostText { get; set; }
    }
}
