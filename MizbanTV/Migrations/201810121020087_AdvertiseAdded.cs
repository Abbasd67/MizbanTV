namespace MizbanTV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertiseAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advertises",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(),
                        FileName = c.String(),
                        Link = c.String(),
                        AdvertiseType = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Advertises");
        }
    }
}
