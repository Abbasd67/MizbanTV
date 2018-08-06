namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderForCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Order", c => c.Int(nullable: false));
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "Name", c => c.String());
            DropColumn("dbo.Categories", "Order");
        }
    }
}
