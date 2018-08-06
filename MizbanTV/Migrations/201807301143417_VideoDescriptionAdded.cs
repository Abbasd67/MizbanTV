namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoDescriptionAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "Description", c => c.String());
            AlterColumn("dbo.Videos", "Size", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Videos", "Size", c => c.Int(nullable: false));
            DropColumn("dbo.Videos", "Description");
        }
    }
}
