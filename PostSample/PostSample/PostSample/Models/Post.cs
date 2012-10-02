using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Display(Name = "Post")]
        [MaxLength(500)]
        public string Content { get; set; }

        //[MaxLength(100)]
        //public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        /////
        //public int? UpdatePostId { get; set; }
        //[ForeignKey("UpdatePostId")]
        //public virtual UpdatePost UpdatePost { get; set; }

        //public int? QuestionPostId { get; set; }
        //[ForeignKey("QuestionPostId")]
        //public virtual QuestionPost QuestionPost { get; set; }

        public int TotalComments { get; set; }

        public int? EventPostId { get; set; }
        [ForeignKey("EventPostId")]
        public virtual EventPost EventPost { get; set; }

        public int? PollPostId { get; set; }
        [ForeignKey("PollPostId")]
        public virtual PollPost PollPost { get; set; }
        /////

        public int? PostTypeId { get; set; }
        [ForeignKey("PostTypeId")]
        public virtual PostType PostType { get; set; }

        //For PostComment One-to-Many
        public virtual ICollection<PostComment> PostComments { get; set; }

        //For LikeStatus One-to-Many
        public virtual ICollection<LikeStatus> LikeStatues { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}