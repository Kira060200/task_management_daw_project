namespace Laborator8App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidTaskMig : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskClasses", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.TaskClasses", "WorkerName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskClasses", "WorkerName", c => c.String());
            AlterColumn("dbo.TaskClasses", "Content", c => c.String());
        }
    }
}
