namespace Laborator8App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskClasses", "Status", c => c.String(nullable: false));
            DropColumn("dbo.TaskClasses", "StatusName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskClasses", "StatusName", c => c.String(nullable: false));
            DropColumn("dbo.TaskClasses", "Status");
        }
    }
}
