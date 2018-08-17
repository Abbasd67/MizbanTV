namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HitsAddedForVideos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "Hits", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "Hits");
        }
    }
}
