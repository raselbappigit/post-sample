using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class EventAttendPerson
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public int? EventAttendTypeId { get; set; }
        [ForeignKey("EventAttendTypeId")]
        public virtual EventAttendType EventAttendType { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int? EventPostId { get; set; }
        [ForeignKey("EventPostId")]
        public virtual EventPost EventPost { get; set; }

        //many to many relationship with user
        //public virtual ICollection<User> Users { get; set; }

    }
}