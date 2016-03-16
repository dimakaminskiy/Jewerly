using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jewerly.Domain.Repository
{
   public  class ProductsRepository:IGenericRepository<Product>
    {

        private readonly ApplicationDbContext db;

        public ProductsRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IQueryable<Product> GetAll()
        {
            return db.Products
                .Include(p => p.Discount)
                .Include(p => p.Markup)
                .Include(p => p.Picture)
                .Include(p => p.MappingProductChoiceAttributeToProducts.Select(t => t.ProductChoiceAttribute))
                .Include(
                    p =>
                        p.MappingProductChoiceAttributeToProducts.Select(
                            t => t.AllowSpecificationAttributeOptionToProduct.Select(x => x.ChoiceAttributeOption)))
                .Include(
                    p => p.MappingProductSpecificationAttributeToProducts.Select(t => t.ProductSpecificationAttribute))
                .Include(
                    p => p.MappingProductSpecificationAttributeToProducts.Select(t => t.SpecificationAttributeOption));
        }

        public  async Task<List<Product>> GetAllAsync()
        {
            return await db.Products
                .Include(p => p.Discount)
                .Include(p => p.Markup)
                .Include(p => p.Picture)
                .Include(p => p.MappingProductChoiceAttributeToProducts.Select(t => t.ProductChoiceAttribute))
                .Include(
                    p =>
                        p.MappingProductChoiceAttributeToProducts.Select(
                            t => t.AllowSpecificationAttributeOptionToProduct.Select(x => x.ChoiceAttributeOption)))
                .Include(
                    p => p.MappingProductSpecificationAttributeToProducts.Select(t => t.ProductSpecificationAttribute))
                .Include(
                    p => p.MappingProductSpecificationAttributeToProducts.Select(t => t.SpecificationAttributeOption))
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await db.Products
                .Include(p => p.Discount)
                .Include(p => p.Markup)
                .Include(p => p.Picture)
                .Include(p => p.MappingProductChoiceAttributeToProducts.Select(t => t.ProductChoiceAttribute))
                .Include(
                    p =>
                        p.MappingProductChoiceAttributeToProducts.Select(
                            t => t.AllowSpecificationAttributeOptionToProduct.Select(x => x.ChoiceAttributeOption)))
                .Include(
                    p => p.MappingProductSpecificationAttributeToProducts.Select(t => t.ProductSpecificationAttribute))
                .Include(
                    p => p.MappingProductSpecificationAttributeToProducts.Select(t => t.SpecificationAttributeOption)).SingleOrDefaultAsync(i => i.ProductId == id);


        }

        public async Task EditAsync(Product entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task InsertAsync(Product entity)
        {
            db.Products.Add(entity);
            await db.SaveChangesAsync();
        }

        public  async Task DeleteAsync(Product entity)
        {
            db.Products.Remove(entity);
            await db.SaveChangesAsync();
        }

        public IQueryable<Product> SearchFor(Expression<Func<Product, bool>> predicate)
        {
            return db.Products.Where(predicate);
        }

        public  async Task<List<Product>> SearchForAsync(Expression<Func<Product, bool>> predicate)
        {
            return await db.Products.Where(predicate).ToListAsync();
        }

        public Product GetById(int id)
        {
            return  db.Products
              .Include(p => p.Discount)
              .Include(p => p.Markup)
              .Include(p => p.Picture)
              .Include(p => p.MappingProductChoiceAttributeToProducts.Select(t => t.ProductChoiceAttribute))
              .Include(
                  p =>
                      p.MappingProductChoiceAttributeToProducts.Select(
                          t => t.AllowSpecificationAttributeOptionToProduct.Select(x => x.ChoiceAttributeOption)))
              .Include(
                  p => p.MappingProductSpecificationAttributeToProducts.Select(t => t.ProductSpecificationAttribute))
              .Include(
                  p => p.MappingProductSpecificationAttributeToProducts.Select(t => t.SpecificationAttributeOption)).SingleOrDefault(i => i.ProductId == id);

        }

        public void Edit(Product entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Insert(Product entity)
        {
            db.Products.Add(entity);
            db.SaveChanges();
        }

        public void Delete(Product entity)
        {
            db.Products.Remove(entity);
            db.SaveChanges();
        }

        public async Task<int> CountAsync()
        {
            return await db.Products.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<Product, bool>> predicate)
        {
            return await db.Products.Where(predicate).CountAsync();
        }

       public int Count(Expression<Func<Product, bool>> predicate)
       {
           return db.Products.Where(predicate).Count();
       }
    }
}
