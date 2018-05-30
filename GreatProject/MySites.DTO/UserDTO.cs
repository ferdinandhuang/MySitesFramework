using MySites.Common.Enum;
using MySites.DataModels;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySites.DTO
{
    public class UserDTO
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType Role { get; set; }
    }

    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public static class UserMapper
    {
        public static UserDTO user2DTO(this User user)
        {
            TinyMapper.Bind<User, UserDTO>();

            return TinyMapper.Map<UserDTO>(user);
        }
    }
}
