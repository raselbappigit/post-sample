using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class PostType
    {
        [Key]
        public int PostTypeId { get; set; }

        public string PostTypeName { get; set; }

    }
}