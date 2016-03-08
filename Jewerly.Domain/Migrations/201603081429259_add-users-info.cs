namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addusersinfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "MiddleName", c => c.String());
            AddColumn("dbo.AspNetUsers", "CountryId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CurrencyId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "City", c => c.String());
            AddColumn("dbo.AspNetUsers", "KindOfActivity", c => c.String());
            CreateIndex("dbo.AspNetUsers", "CountryId");
            CreateIndex("dbo.AspNetUsers", "CurrencyId");
            AddForeignKey("dbo.AspNetUsers", "CountryId", "dbo.Countries", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "CurrencyId", "dbo.Currencies", "CurrencyId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.AspNetUsers", "CountryId", "dbo.Countries");
            DropIndex("dbo.AspNetUsers", new[] { "CurrencyId" });
            DropIndex("dbo.AspNetUsers", new[] { "CountryId" });
            DropColumn("dbo.AspNetUsers", "KindOfActivity");
            DropColumn("dbo.AspNetUsers", "City");
            DropColumn("dbo.AspNetUsers", "CurrencyId");
            DropColumn("dbo.AspNetUsers", "CountryId");
            DropColumn("dbo.AspNetUsers", "MiddleName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropTable("dbo.Countries");
        }
    }
}
