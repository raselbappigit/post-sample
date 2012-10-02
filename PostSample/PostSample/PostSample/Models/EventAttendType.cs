using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class EventAttendType
    {
        [Key]
        public int EventAttendTypeId { get; set; }
        
        [MaxLength(50)]
        public string AttendTypeName { get; set; }

    }
}