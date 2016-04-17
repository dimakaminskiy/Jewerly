namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class json : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "ChoiceAttributesInJson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "ChoiceAttributesInJson");
        }
    }
}
