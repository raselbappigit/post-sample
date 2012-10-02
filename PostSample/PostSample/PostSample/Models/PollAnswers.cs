using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class PollAnswers
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(50)]
        public string AnswerContent { get; set; }

        public int AnswerCount { get; set; }

        public int PollPostId { get; set; }
        [ForeignKey("PollPostId")]
        public virtual PollPost PollPost { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}