namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BackGroundAddedToCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "BackgroundImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "BackgroundImage");
        }
    }
}
