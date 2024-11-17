using Ems.DataAccessLayer.Abstract;
using Ems.DataAccessLayer.EntityFrameworkCore.Contexts;
using Ems.Entity.Accounds;
using Ems.Entity.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ems.DataAccessLayer.EntityFrameworkCore.Concrete;
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly EmsContext _emsContext;
    protected DbSet<T> EmsEntity => _emsContext.Set<T>();

    public GenericRepository(EmsContext emsContext)
    {
        _emsContext = emsContext;
    }
    public async Task<bool> AddAsync(T entity)
    {
        var addedState = await EmsEntity.AddAsync(entity);
        return addedState.State == EntityState.Added;
    }

    public async Task<IEnumerable<T>> GetAllAsync(bool tracking=false)
    {
        if (tracking is false)
        {
            return await EmsEntity.AsNoTracking().ToListAsync();
        }
        return await EmsEntity.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await EmsEntity.FindAsync(id);
    } 


    public IQueryable<T> GetWhereAsync(Expression<Func<T, bool>> expression)
        => EmsEntity.Where(expression);

    public bool Remove(T entity)
    {
        var removed = EmsEntity.Remove(entity);
        return removed.State == EntityState.Deleted;
    }


    public bool Update(T entity)
    {
        _emsContext.Entry(entity).State = EntityState.Modified;
        return true;
    }

    public async Task SaveChangesAsync(int? userId = null)
    {
        IEnumerable<EntityEntry<EditedBaseEntity>> entries = _emsContext.ChangeTracker.Entries<EditedBaseEntity>();

        foreach (var entityEntry in entries)
        {
            switch (entityEntry.State)
            {

                case EntityState.Modified:
                    entityEntry.Property(x => x.UpdatedDate).CurrentValue = DateTime.UtcNow;
                    entityEntry.Property(x => x.UpdatedId).CurrentValue = userId ?? 0;
                    break;
                case EntityState.Added:
                    entityEntry.Property(x => x.CreatedDate).CurrentValue = DateTime.UtcNow;
                    entityEntry.Property(x => x.CreatedId).CurrentValue = userId ?? 0;
                    break;
            }
        }  
        
        await _emsContext.SaveChangesAsync();   
    }

    public async Task<IDictionary<TKey, TElement>> GetDictionaryAsync<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> valueSelector)
    {
        return await EmsEntity.ToDictionaryAsync(keySelector, valueSelector);
    }

}
