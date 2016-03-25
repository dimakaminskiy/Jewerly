namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "UserId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "UserId");
        }
    }
}
