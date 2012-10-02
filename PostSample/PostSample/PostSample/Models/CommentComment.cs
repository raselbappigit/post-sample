using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class CommentComment
    {
        [Key]
        public int CommentOnId { get; set; }

        [Display(Name = "Post")]
        [MaxLength(500)]
        public string Content { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        //For Post One-to-Many
        public int PostCommentId { get; set; }
        [ForeignKey("PostCommentId")]
        public virtual PostComment PostComment { get; set; }

        //For LikeStatus One-to-Many
        public virtual ICollection<LikeStatus> LikeStatues { get; set; }

        public int? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profile UserProfile { get; set; }
    }
}