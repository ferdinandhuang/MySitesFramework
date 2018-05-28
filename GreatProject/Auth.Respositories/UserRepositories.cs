using System;
using System.Collections.Generic;
using Auth.IRespositories;
using EntityFrameworkCore.Triggers;
using Framework.Core.Attributes;
using Framework.Core.Cache;
using Framework.Core.Common;
using Framework.Core.DbContextCore.CodeFirst;
using Framework.Core.Extensions;
using Framework.Core.Repositories;
using MySites.DataModels;

namespace Auth.Respositories
{
    public class UserRepositories : BaseRepository<User>, IUserRepositories
    {
        public UserRepositories(IDbContextCore dbContext) : base(dbContext)
        {
            //插入成功后触发
            Triggers<User>.Inserted += async entry =>
            {

                await entry.Context.SaveChangesWithTriggersAsync(entry.Context.SaveChangesAsync);
                await DistributedCacheManager.RemoveAsync("Redis_Cache_User");//插入成功后清除缓存以更新
            };
            //修改时触发
            Triggers<User>.Updating += entry =>
            {

            };
        }

        public void AddTest()
        {
            var user = new User()
            {
                UserName = "",
                Name = "",
                Passwords = "",
            };

            var a = GetSingle("");
            var b = Add(user);
        }

        [RedisCache(Expiration = 5)]
        public Result<User> Search(string username, string passwords)
        {
            var result = new Result<User>();
            result.Data = new User();

            var user = GetSingleOrDefault(s => s.UserName == username && s.Passwords == passwords);

            if(user == null)
            {
                result.Message = "用户名或密码错误！";
                result.Status = Status.Failed;
            }

            result.Status = Status.Success;
            result.Data = user;
            return result;
        }
    }
}
