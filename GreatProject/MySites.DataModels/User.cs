using Framework.Core.Models;
using MySites.Common.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySites.DataModels
{
    [Table("User")]
    [Serializable]
    public class User : BaseModel
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
        /// 密码：SHA256
        /// </summary>
        public string Passwords { get; set; }
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
}
