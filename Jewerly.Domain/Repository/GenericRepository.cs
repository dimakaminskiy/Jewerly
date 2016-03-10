using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jewerly.Domain.Repository
{

        public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
        {
            protected DbSet<TEntity> DbSet;

            private readonly ApplicationDbContext _dbContext;

            public GenericRepository(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
                DbSet = _dbContext.Set<TEntity>();
            }

            public IQueryable<TEntity> GetAll()
            {
                return DbSet;
            }

            public async Task<List<TEntity>> GetAllAsync()
            {
                return await DbSet.ToListAsync();
            }

            public async Task<TEntity> GetByIdAsync(int id)
            {
                return await DbSet.FindAsync(id);
            }

            public async Task EditAsync(TEntity entity)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }

            public async Task InsertAsync(TEntity entity)
            {
                DbSet.Add(entity);
                await _dbContext.SaveChangesAsync();
            }

            public void Insert(TEntity entity)
            {
                DbSet.Add(entity);
                _dbContext.SaveChanges();
            }

            public async Task DeleteAsync(TEntity entity)
            {
                DbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }

            public void Delete(TEntity entity)
            {
                DbSet.Remove(entity);
                _dbContext.SaveChanges();
            }


            public IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
            {
                return DbSet.Where(predicate);
            }

            public async Task<List<TEntity>> SearchForAsync(Expression<Func<TEntity, bool>> predicate)
            {
                return await DbSet.Where(predicate).ToListAsync();
            }

            public TEntity GetById(int id)
            {
                return  DbSet.Find(id);
            }

            public void Edit(TEntity entity)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }

            public async Task<int> CountAsync()
            {
                return await DbSet.CountAsync();
            }

            public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
            {
                return await DbSet.Where(predicate).CountAsync();
            }

            public int Count(Expression<Func<TEntity, bool>> predicate)
            {
                return  DbSet.Where(predicate).Count();
            }
        }
}
