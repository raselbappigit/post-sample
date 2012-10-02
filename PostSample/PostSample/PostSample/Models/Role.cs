using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PostSample.Models
{
    public class Role
    {
        [Key]
        [Display(Name = "Role Name")]
        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}