namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeVideoFreignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Videos", "Category_ID", "dbo.Categories");
            DropIndex("dbo.Videos", new[] { "Category_ID" });
            RenameColumn(table: "dbo.Videos", name: "Category_ID", newName: "CategoryID");
            AlterColumn("dbo.Videos", "CategoryID", c => c.Guid(nullable: false));
            CreateIndex("dbo.Videos", "CategoryID");
            AddForeignKey("dbo.Videos", "CategoryID", "dbo.Categories", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Videos", new[] { "CategoryID" });
            AlterColumn("dbo.Videos", "CategoryID", c => c.Guid());
            RenameColumn(table: "dbo.Videos", name: "CategoryID", newName: "Category_ID");
            CreateIndex("dbo.Videos", "Category_ID");
            AddForeignKey("dbo.Videos", "Category_ID", "dbo.Categories", "ID");
        }
    }
}
