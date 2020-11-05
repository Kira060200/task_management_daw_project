using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Proiect2020.Models
{
    public class AppContext: DbContext
    {
        public AppContext() : base("DBConnectionString") {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext,Proiect2020.Migrations.Configuration>("DBConnectionString"));

        }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}