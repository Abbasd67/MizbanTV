namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DontKnow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "Title", c => c.String());
            AddColumn("dbo.Videos", "FileName", c => c.String());
            DropColumn("dbo.Videos", "Name");
            DropColumn("dbo.Videos", "Path");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Videos", "Path", c => c.String());
            AddColumn("dbo.Videos", "Name", c => c.String());
            DropColumn("dbo.Videos", "FileName");
            DropColumn("dbo.Videos", "Title");
        }
    }
}
