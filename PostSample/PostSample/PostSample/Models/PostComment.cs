using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class PostComment
    {
        [Key]
        public int CommentId { get; set; }

        [Display(Name = "Post")]
        [MaxLength(500)]
        public string Content { get; set; }

        //[MaxLength(100)]
        //public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        //For Post One-to-Many
        public int? PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        //public int? PerantFlagId { get; set; }

        //For CommentComment One-to-Many
        public virtual PostComment PerantPostComment { get; set; }

        public virtual ICollection<PostComment> PostCommentChilds { get; set; }

        //For LikeStatus One-to-Many
        public virtual ICollection<LikeStatus> LikeStatues { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}