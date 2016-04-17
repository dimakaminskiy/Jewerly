namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class json2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "ChoiceAttributesInString", c => c.String());
            DropColumn("dbo.OrderDetails", "ChoiceAttributesInJson");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "ChoiceAttributesInJson", c => c.String());
            DropColumn("dbo.OrderDetails", "ChoiceAttributesInString");
        }
    }
}
