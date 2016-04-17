namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _43 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "ChoiceAttributesInString", c => c.String());
            AddColumn("dbo.OrderDetails", "ChoiceAttributesInString", c => c.String());
            DropColumn("dbo.OrderDetails", "ChoiceAttributesInJson");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "ChoiceAttributesInJson", c => c.String());
            DropColumn("dbo.OrderDetails", "ChoiceAttributesInString");
            DropColumn("dbo.Carts", "ChoiceAttributesInString");
        }
    }
}
