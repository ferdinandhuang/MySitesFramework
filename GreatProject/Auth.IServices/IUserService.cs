using Framework.Core.Extensions;
using MySites.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.IServices
{
    public interface IUserService
    {
        Result<UserDTO> SearchUserByPwd(string username, string passwords);
    }
}
