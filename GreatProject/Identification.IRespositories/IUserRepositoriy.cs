using Framework.Core.Attributes;
using Framework.Core.Extensions;
using MySites.DataModels;
using MySites.DTO;
using System.Collections.Generic;

namespace Identification.IRepositories
{
    public interface IUserRepositoriy
    {
        [RedisCache(CacheKey = "Redis_Cache_Users")]
        IList<User> GetUsers();

        Result<string> AddUser(UserDTO userDTO);
    }
}
