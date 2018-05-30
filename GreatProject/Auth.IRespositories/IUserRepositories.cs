using Framework.Core.Attributes;
using Framework.Core.Extensions;
using Framework.Core.Repositories;
using MySites.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Auth.IRespositories
{
    public interface IUserRepositories : IRepository<User>
    {
        void AddTest();
        
        Result<User> Search(string username, string passwords);

        [RedisCache(CacheKey = "Redis_Cache_User", Expiration = 5)]
        IList<User> GetByRedisCached(Expression<Func<User, bool>> where = null);
    }
}
