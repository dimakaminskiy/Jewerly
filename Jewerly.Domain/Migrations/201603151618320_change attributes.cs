namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeattributes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpecificationAttributeOptions", "SeoName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpecificationAttributeOptions", "SeoName");
        }
    }
}
