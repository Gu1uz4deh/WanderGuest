using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.Entity;

namespace WanderQuest.Core.EFRepository
{
    public interface IRepositoryBase<TEntity> where TEntity : class, IEntity, new()
    {
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression = null,
                        params Expression<Func<TEntity, object>>[] includes);

        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null,
                                 params Expression<Func<TEntity, object>>[] includes);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
