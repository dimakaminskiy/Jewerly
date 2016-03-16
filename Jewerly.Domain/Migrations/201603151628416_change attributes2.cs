namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeattributes2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductSpecificationAttributes", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ProductSpecificationAttributes", "SeoName", c => c.String(nullable: false));
            AlterColumn("dbo.SpecificationAttributeOptions", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SpecificationAttributeOptions", "Name", c => c.String());
            AlterColumn("dbo.ProductSpecificationAttributes", "SeoName", c => c.String());
            AlterColumn("dbo.ProductSpecificationAttributes", "Name", c => c.String());
        }
    }
}
