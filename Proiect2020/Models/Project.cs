using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect2020.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        public int TeamId { get; set; }


        public virtual Team Team { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}