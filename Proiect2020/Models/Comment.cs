﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proiect2020.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public string Content { get; set; }
        //trebuie adaugat user id 
        public DateTime Date { get; set; }
        public int TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}