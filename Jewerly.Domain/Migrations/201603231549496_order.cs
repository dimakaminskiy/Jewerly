namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderDetails", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.OrderDetails", new[] { "CurrencyId" });
            AddColumn("dbo.Orders", "CurrencyId", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.OrderDetails", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Orders", "CurrencyId");
            AddForeignKey("dbo.Orders", "CurrencyId", "dbo.Currencies", "CurrencyId", cascadeDelete: true);
            DropColumn("dbo.OrderDetails", "CurrencyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "CurrencyId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Orders", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.Orders", new[] { "CurrencyId" });
            AlterColumn("dbo.OrderDetails", "UnitPrice", c => c.Double(nullable: false));
            AlterColumn("dbo.Orders", "Total", c => c.Double(nullable: false));
            DropColumn("dbo.Orders", "CurrencyId");
            CreateIndex("dbo.OrderDetails", "CurrencyId");
            AddForeignKey("dbo.OrderDetails", "CurrencyId", "dbo.Currencies", "CurrencyId", cascadeDelete: true);
        }
    }
}
