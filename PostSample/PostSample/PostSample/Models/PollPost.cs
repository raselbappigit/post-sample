using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class PollPost
    {
        [Key]
        public int Id { get; set; }

        public int TotalVote { get; set; }

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public virtual ICollection<PollAnswers> PollAnswers { get; set; }
    }
}