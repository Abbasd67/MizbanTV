namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateAddedToVideo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Videos", "LastModifiedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "LastModifiedDate");
            DropColumn("dbo.Videos", "CreateDate");
        }
    }
}
