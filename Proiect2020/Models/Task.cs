using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect2020.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Status { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }


        public virtual Project Project { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}