using Chloe;
using System;

namespace Framework.Core.DbContextCore.DbFirst
{
    public abstract class BaseRepository : IDisposable
    {
        protected readonly IDbContext Db;
        public BaseRepository(IDbContext _dbContext)
        {
            Db = _dbContext;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Db?.Dispose();
                }

                disposedValue = true;
            }
        }

        ~BaseRepository()
        {
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
