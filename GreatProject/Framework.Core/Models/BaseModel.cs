using Framework.Core.Common;
using Framework.Core.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.Core.Models
{
    public interface IAggregateRoot
    {

    }

    /// <summary>
    /// 所有数据表实体类都必须实现此接口
    /// </summary>
    [Serializable]
    public class BaseModel : IAggregateRoot
    {
        public BaseModel()
        {
            Id = CommonHelper.NewMongodbId().ToString();
            Flag = Flag.Normal;
            CreateTime = DateHelper.GetTimeStamp().ToString();
            UpdateTime = DateHelper.GetTimeStamp().ToString();
            CreateUser = "-";
            UpdateUser = "-";
        }
        /// <summary>
        /// GUID
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 标志位
        /// </summary>
        public Flag Flag { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateUser { get; set; }
    }
}
