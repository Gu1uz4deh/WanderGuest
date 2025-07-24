using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.Entity;

namespace WanderQuest.Core.EFRepository.EFEntityRepositoryBase
{
    public class EFEntityRepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext
    {
        protected readonly TContext _context;

        public EFEntityRepositoryBase(TContext context)
        {
            _context = context;
        }
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().AsNoTracking();
            // var data = expression is null
            //     ? await query.FirstOrDefaultAsync()
            //     : await query.Where(expression).FirstOrDefaultAsync();
            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.FirstOrDefault();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null, 
                                              params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().AsNoTracking();
            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            var data = _context.Entry(entity);
            data.State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var data = _context.Entry(entity);
            data.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            var data = _context.Entry(entity);
            data.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
