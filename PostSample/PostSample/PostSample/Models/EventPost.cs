using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class EventPost
    {
        [Key]
        public int Id { get; set; }

        public DateTime EventDateTime { get; set; }

        [MaxLength(20)]
        public string EventDuration { get; set; }

        [MaxLength(20)]
        public string EventVenue { get; set; }

        [MaxLength(20)]
        public string EventNote { get; set; }

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public virtual ICollection<EventAttendPerson> EventAttendPersons { get; set; }
    }
}