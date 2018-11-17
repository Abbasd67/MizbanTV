namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        VideoID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Text = c.String(nullable: false, storeType: "ntext"),
                        IsApproved = c.Boolean(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Videos", t => t.VideoID, cascadeDelete: true)
                .Index(t => t.VideoID);
            
            AddColumn("dbo.Videos", "IsActivated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "VideoID", "dbo.Videos");
            DropIndex("dbo.Comments", new[] { "VideoID" });
            DropColumn("dbo.Videos", "IsActivated");
            DropTable("dbo.Comments");
        }
    }
}
