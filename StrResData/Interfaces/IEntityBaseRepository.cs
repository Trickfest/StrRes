using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StrResData.Interfaces
{
    public interface IEntityBaseRepository<T> where T : class, new()
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate);
        Task<T> GetSingle(params object[] keyValues);
        Task Add(ref T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}