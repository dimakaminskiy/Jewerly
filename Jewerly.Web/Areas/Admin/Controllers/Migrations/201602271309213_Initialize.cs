namespace Jewerly.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        CurrencyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CurrencyCode = c.String(),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CurrencyId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        SeoFilename = c.String(),
                        AltAttribute = c.String(),
                        TitleAttribute = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductAttributes",
                c => new
                    {
                        ProductAttributeId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductAttributeId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductDropDownAttributeOptions",
                c => new
                    {
                        ProductDropDownAttributeOptionId = c.Int(nullable: false, identity: true),
                        ProductDropDownAttributeId = c.Int(nullable: false),
                        Name = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductDropDownAttributeOptionId)
                .ForeignKey("dbo.ProductDropDownAttributes", t => t.ProductDropDownAttributeId, cascadeDelete: true)
                .Index(t => t.ProductDropDownAttributeId);
            
            CreateTable(
                "dbo.ProductDropDownAttributes",
                c => new
                    {
                        ProductDropDownAttributeId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Name = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductDropDownAttributeId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SeoName = c.String(),
                        ShortDescription = c.String(),
                        FullDescription = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Published = c.Boolean(nullable: false),
                        PictureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Pictures", t => t.PictureId, cascadeDelete: true)
                .Index(t => t.PictureId);
            
            CreateTable(
                "dbo.ProductSpecificationAttributes",
                c => new
                    {
                        ProductSpecificationAttributeId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Name = c.String(),
                        AllowFiltering = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductSpecificationAttributeId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.SpecificationAttributeOptions",
                c => new
                    {
                        SpecificationAttributeOptionId = c.Int(nullable: false, identity: true),
                        SpecificationAttributeId = c.Int(nullable: false),
                        Name = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                        ProductSpecificationAttribute_ProductSpecificationAttributeId = c.Int(),
                    })
                .PrimaryKey(t => t.SpecificationAttributeOptionId)
                .ForeignKey("dbo.ProductSpecificationAttributes", t => t.ProductSpecificationAttribute_ProductSpecificationAttributeId)
                .Index(t => t.ProductSpecificationAttribute_ProductSpecificationAttributeId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProductDropDownAttributeOptions", "ProductDropDownAttributeId", "dbo.ProductDropDownAttributes");
            DropForeignKey("dbo.SpecificationAttributeOptions", "ProductSpecificationAttribute_ProductSpecificationAttributeId", "dbo.ProductSpecificationAttributes");
            DropForeignKey("dbo.ProductSpecificationAttributes", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductDropDownAttributes", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductAttributes", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "PictureId", "dbo.Pictures");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.SpecificationAttributeOptions", new[] { "ProductSpecificationAttribute_ProductSpecificationAttributeId" });
            DropIndex("dbo.ProductSpecificationAttributes", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "PictureId" });
            DropIndex("dbo.ProductDropDownAttributes", new[] { "ProductId" });
            DropIndex("dbo.ProductDropDownAttributeOptions", new[] { "ProductDropDownAttributeId" });
            DropIndex("dbo.ProductAttributes", new[] { "ProductId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.SpecificationAttributeOptions");
            DropTable("dbo.ProductSpecificationAttributes");
            DropTable("dbo.Products");
            DropTable("dbo.ProductDropDownAttributes");
            DropTable("dbo.ProductDropDownAttributeOptions");
            DropTable("dbo.ProductAttributes");
            DropTable("dbo.Pictures");
            DropTable("dbo.Currencies");
        }
    }
}
