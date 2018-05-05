using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Common
{
    /// <summary>
    /// 标志位
    /// </summary>
    public enum Flag
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 逻辑删除
        /// </summary>
        Droped = 2,
    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 失败
        /// </summary>
        Failed = 2,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 3,
    }
}
