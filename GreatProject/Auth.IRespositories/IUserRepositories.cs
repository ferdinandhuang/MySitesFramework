using Framework.Core.Attributes;
using Framework.Core.Extensions;
using Framework.Core.Repositories;
using MySites.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Auth.IRespositories
{
    public interface IUserRepositories : IRepository<User>
    {
        void AddTest();

        [RedisCache(Expiration = 5)]
        Result<User> Search(string username, string passwords);

        [RedisCache(CacheKey = "Redis_Cache_User", Expiration = 5)]
        IList<User> GetByRedisCached(Expression<Func<User, bool>> where = null);
    }
}
