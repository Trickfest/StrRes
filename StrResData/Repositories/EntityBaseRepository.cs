using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StrResData.Entities;
using StrResData.Interfaces;

namespace StrResData.Repositories
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : StrResData.Entities.BaseEntity, new()
    {
        protected StrResDbContext _context;

        public EntityBaseRepository(StrResDbContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate){
            return _context.Set<T>().Where(predicate);
        }

        virtual public Task<T> GetSingle(params object[] keyValues)
        {
            return _context.Set<T>().FindAsync(keyValues);
        }

        public Task Add(ref T entity)
        {
            entity.CreatedTime = entity.ModifiedTime = DateTime.Now;
            _context.Set<T>().Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task Update(T entity)
        {
            entity.ModifiedTime = DateTime.Now;
            _context.Entry(entity).State = EntityState.Modified;

            // avoid updating the CreatedTime property
            _context.Entry(entity).Property(nameof(entity.CreatedTime)).IsModified = false;
            
            return _context.SaveChangesAsync();
        }

        public Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return _context.SaveChangesAsync();
        }
    }
}