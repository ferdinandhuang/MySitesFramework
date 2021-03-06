﻿using Auth.IRespositories;
using Auth.IServices;
using Chloe;
using Framework.Core.Common;
using Framework.Core.Extensions;
using Identification.IRepositories;
using MySites.DataModels;
using MySites.DTO;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth.Services
{
    public class UserService : IUserService
    {
        IUserRepositoriy userRepositoriy;
        public UserService(IUserRepositoriy _userRepositoriy)
        {
            userRepositoriy = _userRepositoriy;
        }
        public Result<UserDTO> SearchUserByPwd(string username, string passwords)
        {
            var result = new Result<UserDTO>();
            
            var user = userRepositoriy.GetUsers().FirstOrDefault(s => s.UserName == username && s.Passwords == passwords);

            if (user == null)
            {
                result.Message = "用户名或密码错误！";
                result.Status = Status.Failed;
                return result;
            }

            var userDTO = user.user2DTO();

            result.Status = Status.Success;
            result.Data = userDTO;
            return result;
        }
    }
}
