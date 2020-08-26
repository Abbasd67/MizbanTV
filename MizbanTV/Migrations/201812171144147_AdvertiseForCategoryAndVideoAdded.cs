namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertiseForCategoryAndVideoAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "AdvertiseFileName", c => c.String());
            AddColumn("dbo.Categories", "AdvertiseLink", c => c.String());
            AddColumn("dbo.Videos", "AdvertiseFileName", c => c.String());
            AddColumn("dbo.Videos", "AdvertiseLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "AdvertiseLink");
            DropColumn("dbo.Videos", "AdvertiseFileName");
            DropColumn("dbo.Categories", "AdvertiseLink");
            DropColumn("dbo.Categories", "AdvertiseFileName");
        }
    }
}
