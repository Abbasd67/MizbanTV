namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoAndCategoryAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        Size = c.Int(nullable: false),
                        Path = c.String(),
                        Category_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.Category_ID)
                .Index(t => t.Category_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "Category_ID", "dbo.Categories");
            DropIndex("dbo.Videos", new[] { "Category_ID" });
            DropTable("dbo.Videos");
            DropTable("dbo.Categories");
        }
    }
}
