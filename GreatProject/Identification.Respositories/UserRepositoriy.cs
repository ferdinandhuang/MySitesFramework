using Chloe;
using Framework.Core.Common;
using Framework.Core.DbContextCore.DbFirst;
using Framework.Core.Extensions;
using Identification.IRepositories;
using MySites.DataModels;
using MySites.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identification.Repositories
{
    public class UserRepositoriy : BaseRepository, IUserRepositoriy
    {
        public UserRepositoriy(IDbContext _dbContext) : base(_dbContext)
        {
        }

        public Result<string> AddUser(UserDTO userDTO)
        {
            var ret = new Result<string>();
            var user = new User();
            try
            {
                user = userDTO.DTO2User();
                Db.Insert(user);
            }
            catch(Exception ex)
            {
                ret.Message = ex.Message;
                ret.Status = Status.Error;
                return ret;
            }

            ret.Data = user.Id;
            ret.Status = Status.Success;
            return ret;
        }

        public IList<User> GetUsers()
        {
            var users = Db.Query<User>().ToList();

            return users;
        }
    }
}
