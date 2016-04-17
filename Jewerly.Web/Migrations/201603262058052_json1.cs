namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class json1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "ChoiceAttributesInString", c => c.String());
            DropColumn("dbo.Carts", "ChoiceAttributesInJson");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Carts", "ChoiceAttributesInJson", c => c.String());
            DropColumn("dbo.Carts", "ChoiceAttributesInString");
        }
    }
}
