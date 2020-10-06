using System;
using System.ComponentModel.DataAnnotations;

namespace BBKBootcampSocial.Domains.Common
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
