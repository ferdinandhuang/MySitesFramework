using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Auth.IRespositories;
using EntityFrameworkCore.Triggers;
using Framework.Core.Attributes;
using Framework.Core.Cache;
using Framework.Core.Common;
using Framework.Core.DbContextCore.CodeFirst;
using Framework.Core.Extensions;
using Framework.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using MySites.DataModels;
using MySites.DTO;
using Nelibur.ObjectMapper;

namespace Auth.Respositories
{
    public class UserRepositories : BaseRepository<User>, IUserRepositories
    {
        public UserRepositories(IDbContextCore dbContext) : base(dbContext)
        {
            //插入成功后触发
            Triggers<User>.Inserted += async entry =>
            {
                await DistributedCacheManager.RemoveAsync("Redis_Cache_User");//插入成功后清除缓存以更新
            };
            //修改时触发
            Triggers<User>.Updating += async entry =>
            {
                await DistributedCacheManager.RemoveAsync("Redis_Cache_User");//插入成功后清除缓存以更新
            };
        }
        
        public IList<User> GetByRedisCached()
        {
            var users = Get().ToList();

            return users;
        }
    }
}
