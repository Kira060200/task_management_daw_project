using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Laborator8App.Models;


namespace Laborator8App.Models
{
    public class Member
    {
        public int MemberId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        //public DateTime RequestTime { get; set; }

        public bool Accepted { get; set; }
        
    }
}