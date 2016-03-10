using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jewerly.Domain.Repository
{
    public interface IGenericRepository<TEntity>
    {
        IQueryable<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task EditAsync(TEntity entity);

        Task InsertAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> SearchForAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity GetById(int id);
        void Edit(TEntity entity);
        void Insert(TEntity entity);
        void Delete(TEntity entity);
        
        
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        int Count(Expression<Func<TEntity, bool>> predicate);

    }
}