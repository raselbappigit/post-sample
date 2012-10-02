using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class FeedPost
    {
        [Key]
        public int FeedPostId { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }

        public int? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profile UserProfile { get; set; }

        public virtual ICollection<FeedPostComment> FeedPostComment { get; set; }

    }
    public class FeedPostComment
    {
        [Key]
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }

        public int? FeedId { get; set; }
        [ForeignKey("FeedId")]
        public virtual FeedPost FeedPost { get; set; }

        public int? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profile UserProfile { get; set; }

        //public int? ParentId { get; set; }
        //[ForeignKey("ParentId")]
        //public virtual FeedPostComment Child { get; set; }

        public virtual ICollection<FeedPostComment> FeedPostCommentChilds { get; set; }
    }
}