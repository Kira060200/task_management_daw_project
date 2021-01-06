
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Laborator8App.Models;
namespace Laborator8App.Models
{
    public class TaskClass
    {
        [Key]
        public int TaskId { get; set; }
        [Required(ErrorMessage = "Titlul este obligatoriu!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Statusul este obligatoriu!")]
        public string Status { get; set; }
        [Required(ErrorMessage = "Descrierea este obligatorie!")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Asignarea este obligatorie!")]
        public string WorkerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}