using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class LikeStatus
    {
        [Key]
        public string LikeId { get; set; }

        //For Post One-to-Many
        public virtual ICollection<Post> Posts { get; set; }

        //For Post Comment One-to-Many
        public virtual ICollection<PostComment> Comments { get; set; }

        //For user One-to-Many
        public virtual ICollection<User> Users { get; set; }
    }
}