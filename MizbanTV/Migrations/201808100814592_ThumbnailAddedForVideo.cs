namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThumbnailAddedForVideo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "ThumbName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "ThumbName");
        }
    }
}
