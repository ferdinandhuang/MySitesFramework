using Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Core.Repositories
{
    public interface IRepository<T> : IQueryable<T>, IDisposable where T : BaseModel
    {
        #region Insert

        int Add(T entity, bool withTrigger = false);
        Task<int> AddAsync(T entity, bool withTrigger = false);
        int AddRange(ICollection<T> entities, bool withTrigger = false);
        Task<int> AddRangeAsync(ICollection<T> entities, bool withTrigger = false);
        void BulkInsert(IList<T> entities, string destinationTableName = null);

        #endregion

        #region Delete

        int Delete(string key, bool withTrigger = false);
        int Delete(Expression<Func<T, bool>> @where);
        Task<int> DeleteAsync(Expression<Func<T, bool>> @where);

        #endregion

        #region Update

        int Edit(T entity, bool withTrigger = false);
        int EditRange(ICollection<T> entities, bool withTrigger = false);
        int BatchUpdate(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateExp);
        Task<int> BatchUpdateAsync(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateExp);
        int Update(T model, bool withTrigger = false, params string[] updateColumns);
        int Update(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory);
        Task<int> UpdateAsync(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory);

        #endregion

        #region Query

        int Count(Expression<Func<T, bool>> @where = null);
        Task<int> CountAsync(Expression<Func<T, bool>> @where = null);
        bool Exist(Expression<Func<T, bool>> @where = null);
        Task<bool> ExistAsync(Expression<Func<T, bool>> @where = null);
        T GetSingle(string key);
        T GetSingle(string key, Func<IQueryable<T>, IQueryable<T>> includeFunc);
        Task<T> GetSingleAsync(string key);
        T GetSingleOrDefault(Expression<Func<T, bool>> @where = null);
        Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> @where = null);
        IQueryable<T> Get(Expression<Func<T, bool>> @where = null);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> @where = null);
        IEnumerable<T> GetByPagination(Expression<Func<T, bool>> @where, int pageSize, int pageIndex, bool asc = true,
            params Func<T, object>[] @orderby);

        #endregion
    }
}
