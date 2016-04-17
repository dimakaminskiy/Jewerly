namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _41 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Categories", "ParentCategoryId");
            AddForeignKey("dbo.Categories", "ParentCategoryId", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "ParentCategoryId", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "ParentCategoryId" });
        }
    }
}
