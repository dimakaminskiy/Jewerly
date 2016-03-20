using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Jewerly.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jewerly.Domain
{

    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public virtual DbSet<Picture> Pictures { get; set; }    
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductSpecificationAttribute> ProductSpecificationAttributes { get; set; }
        public virtual DbSet<SpecificationAttributeOption> SpecificationAttributeOptions { get; set; }
        public virtual DbSet<MappingProductSpecificationAttributeToProduct> MappingProductSpecificationAttributeToProducts { get; set; }

        public virtual DbSet<MappingProductChoiceAttributeToProduct> MappingProductChoiceAttributeToProducts { get; set; }
        public virtual DbSet<AvalibleChoiceAttributeOption> AvalibleChoiceAttributeOptions { get; set; }
        public virtual DbSet<ProductChoiceAttribute> ProductChoiceAttributes { get; set; }
        public virtual DbSet<ChoiceAttributeOption> ChoiceAttributeOptions { get; set; }
        public DbSet<Category> Categories { get; set; }          
        public DbSet<CategoryPicture> CategoryPictures { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Markup> Markups { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Country> Countries { get; set; }



        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<MethodOfDelivery> MethodOfDeliveries { get; set; }
        public DbSet<MethodOfPayment> MethodOfPayments { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<SliderPicture> SliderPictures { get; set; } 

    }
    
}
