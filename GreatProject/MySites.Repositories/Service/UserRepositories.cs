using EntityFrameworkCore.Triggers;
using Framework.Core.Cache;
using Framework.Core.DbContextCore.CodeFirst;
using Framework.Core.Repositories;
using MySites.DataModels;
using MySites.IRepositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySites.Repositories.Service
{
    public class UserRepositories : BaseRepository<User> ,IUserRepositories
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
    }
}
