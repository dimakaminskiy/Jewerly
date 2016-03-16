namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeattributes1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductSpecificationAttributes", "SeoName", c => c.String());
            DropColumn("dbo.SpecificationAttributeOptions", "SeoName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpecificationAttributeOptions", "SeoName", c => c.String());
            DropColumn("dbo.ProductSpecificationAttributes", "SeoName");
        }
    }
}
